using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Minimal GitHub "Contents API" based sync: pulls/pushes a single JSON file
/// to/from a GitHub repo, entirely in memory (no local files).
///
/// Repo config string format: "owner/repo/path/to/file.json"
/// (uses the repo's default branch).
/// </summary>
public static class GitSync
{
    [System.Serializable]
    private class GitContentResponse
    {
        public string content;
        public string sha;
    }

    public static bool TryParseRepoConfig(string config, out string owner, out string repo, out string path)
    {
        owner = repo = path = null;
        if (string.IsNullOrEmpty(config)) return false;

        string[] parts = config.Trim('/').Split('/');
        if (parts.Length < 3) return false;

        owner = parts[0];
        repo = parts[1];

        var pathParts = new string[parts.Length - 2];
        Array.Copy(parts, 2, pathParts, 0, pathParts.Length);
        for (int i = 0; i < pathParts.Length; i++)
            pathParts[i] = Uri.EscapeDataString(pathParts[i]);
        path = string.Join("/", pathParts);

        return true;
    }

    private static string ApiUrl(string owner, string repo, string path)
    {
        return $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
    }

    private static void SetCommonHeaders(UnityWebRequest req, string token)
    {
        req.SetRequestHeader("Authorization", "token " + token);
        req.SetRequestHeader("User-Agent", "CardEditor");
        req.SetRequestHeader("Accept", "application/vnd.github+json");
    }

    /// <summary>
    /// Downloads the configured file and returns its content as a JSON string via onSuccess.
    /// </summary>
    public static IEnumerator Pull(string token, string repoConfig, Action<string> onSuccess, Action<string> onError)
    {
        if (!TryParseRepoConfig(repoConfig, out string owner, out string repo, out string path))
        {
            onError?.Invoke("仓库地址格式不对，应为 owner/repo/path/to/file.json");
            yield break;
        }

        using (UnityWebRequest req = UnityWebRequest.Get(ApiUrl(owner, repo, path)))
        {
            SetCommonHeaders(req, token);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"拉取失败 ({req.responseCode}): {req.error}");
                yield break;
            }

            GitContentResponse res = JsonUtility.FromJson<GitContentResponse>(req.downloadHandler.text);
            if (res == null || string.IsNullOrEmpty(res.content))
            {
                onError?.Invoke("拉取失败: 返回内容为空");
                yield break;
            }

            string base64 = res.content.Replace("\n", "").Replace("\r", "");
            string json;
            try
            {
                json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            }
            catch (Exception e)
            {
                onError?.Invoke("拉取失败: 解码内容出错 " + e.Message);
                yield break;
            }

            onSuccess?.Invoke(json);
        }
    }

    /// <summary>
    /// Uploads `json` to the configured file, overwriting whatever is there.
    /// Always re-fetches the current sha right before pushing, and retries once
    /// if GitHub reports a conflict (409) in between.
    /// </summary>
    public static IEnumerator Push(string token, string repoConfig, string json, Action onSuccess, Action<string> onError)
    {
        if (!TryParseRepoConfig(repoConfig, out string owner, out string repo, out string path))
        {
            onError?.Invoke("仓库地址格式不对，应为 owner/repo/path/to/file.json");
            yield break;
        }

        string url = ApiUrl(owner, repo, path);
        string contentBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        for (int attempt = 0; attempt < 2; attempt++)
        {
            // 1. fetch current sha (file may not exist yet on first push)
            string sha = null;
            using (UnityWebRequest getReq = UnityWebRequest.Get(url))
            {
                SetCommonHeaders(getReq, token);
                yield return getReq.SendWebRequest();

                if (getReq.result == UnityWebRequest.Result.Success)
                {
                    GitContentResponse res = JsonUtility.FromJson<GitContentResponse>(getReq.downloadHandler.text);
                    sha = res?.sha;
                }
                else if (getReq.responseCode != 404)
                {
                    onError?.Invoke($"上传前获取文件信息失败 ({getReq.responseCode}): {getReq.error}");
                    yield break;
                }
                // 404 -> file doesn't exist yet, sha stays null, PUT will create it
            }

            // 2. build request body, omitting "sha" entirely when the file is new
            string message = "CardEditor sync " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string bodyJson = string.IsNullOrEmpty(sha)
                ? $"{{\"message\":\"{message}\",\"content\":\"{contentBase64}\"}}"
                : $"{{\"message\":\"{message}\",\"content\":\"{contentBase64}\",\"sha\":\"{sha}\"}}";
            byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyJson);

            using (UnityWebRequest putReq = new UnityWebRequest(url, "PUT"))
            {
                putReq.uploadHandler = new UploadHandlerRaw(bodyBytes);
                putReq.downloadHandler = new DownloadHandlerBuffer();
                putReq.SetRequestHeader("Content-Type", "application/json");
                SetCommonHeaders(putReq, token);

                yield return putReq.SendWebRequest();

                if (putReq.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke();
                    yield break;
                }

                // someone else updated the file between our GET and PUT: retry once with a fresh sha
                if (putReq.responseCode == 409 && attempt == 0)
                    continue;

                onError?.Invoke($"上传失败 ({putReq.responseCode}): {putReq.downloadHandler.text}");
                yield break;
            }
        }
    }
}
