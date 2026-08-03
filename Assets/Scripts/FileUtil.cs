using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using System.IO;

public class FileUtil
{
    public static string ChooseAJsonFile() {
        OpenFileDialog dialog = new OpenFileDialog();

        dialog.Title = "选择 JSON 文件";
        dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        dialog.Multiselect = false;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            return dialog.FileName;
        }
        return null;
    }

    public static string ChooseAFolder() {
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        dialog.Description = "请选择一个文件夹";
        dialog.ShowNewFolderButton = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            return dialog.SelectedPath;
        }
        return null;
    }

    public static string ChooseAImage()
    {
        OpenFileDialog dialog = new OpenFileDialog();

        dialog.Title = "选择图片";
        dialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
        dialog.Multiselect = false;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            string path = dialog.FileName;
            return path;
            //Debug.Log("选择的图片: " + path);

            //// 读取图片为 Texture2D
            //byte[] bytes = File.ReadAllBytes(path);

            //Texture2D tex = new Texture2D(2, 2);
            //tex.LoadImage(bytes);

            //// 测试：打印尺寸
            //Debug.Log($"图片尺寸: {tex.width}x{tex.height}");

            //// 你可以把 tex 赋给 UI RawImage / 材质等
        }
        return null;
    }

    public static void SaveJson(string json)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "JSON files (*.json)|*.json";
        dialog.Title = "Save JSON File";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(dialog.FileName, json);
            Debug.Log("Saved to: " + dialog.FileName);
        }
    }

    public static string SaveExcel() {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
        dialog.Title = "Save Excel File";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            return dialog.FileName;
        }
        else { 
            return null;
        }
    }

    public static void LoadJson()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "JSON files (*.json)|*.json";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            string json = File.ReadAllText(dialog.FileName);
            Data.work = JsonUtility.FromJson<Work>(json);
        }
    }
}

