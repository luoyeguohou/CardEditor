using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        private const string PrefKeyToken = "CardEditor_GitToken";
        private const string PrefKeyRepo = "CardEditor_GitRepo";

        private string pullTitle;
        private string pushTitle;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnSaveWork.onClick.Add(OnClickSaveWork);
            m_cont.m_btnLoadWork.onClick.Add(OnClickLoadWork);
            m_cont.m_btnDefBlock.onClick.Add(OnClickDefBlock);
            m_cont.m_btnEditCard.onClick.Add(OnClickEditCard);
            m_cont.m_btnExport.onClick.Add(OnClickExport);
            m_cont.m_btnChangePath.onClick.Add(OnClickChooseFolder);
            m_cont.m_btnPull.onClick.Add(OnClickPull);
            m_cont.m_btnPush.onClick.Add(OnClickPush);

            pullTitle = m_cont.m_btnPull.title;
            pushTitle = m_cont.m_btnPush.title;

            m_cont.m_txtToken.text = PlayerPrefs.GetString(PrefKeyToken, "");
            m_cont.m_txtGitRepo.text = PlayerPrefs.GetString(PrefKeyRepo, "");
        }

        public void Init() {
            m_cont.m_platform.selectedIndex = Application.isMobilePlatform ? 1 : 0;
        }
        private void OnClickSaveWork()
        {
            Data.work.PrepareForSave();
            string json = JsonUtility.ToJson(Data.work, true);
            FileUtil.SaveJson(json);
        }
        private void OnClickLoadWork()
        {
            FileUtil.LoadJson();
            Data.work.ResolveLinks();
            m_cont.m_txtExport.text = Data.work.export;
        }
        private void OnClickDefBlock()
        {
            FGUIUtil.CreateWindow<UI_BlockWin>("BlockWin").InitView();
        }
        private void OnClickEditCard()
        {
            FGUIUtil.CreateWindow<UI_CardListWin>("CardListWin").UpdateView();
        }

        private void OnClickExport() {
            CreateExcel();
        }
        private void OnClickChooseFolder() {
            Data.work.export = FileUtil.ChooseAFolder();
            m_cont.m_txtExport.text = Data.work.export;
        }

        void CreateExcel()
        {
            string path = Path.Combine(Application.dataPath, "Test.xlsx");

            FileInfo file = new FileInfo(path);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet effectSheet = package.Workbook.Worksheets.Add("Effects");
                ExcelWorksheet cardSheet = package.Workbook.Worksheets.Add("Cards");

                effectSheet.Cells[1, 1].Value = "id";
                effectSheet.Cells[1, 2].Value = "action";
                effectSheet.Cells[1, 3].Value = "linked_1";
                effectSheet.Cells[1, 4].Value = "linked_2";
                effectSheet.Cells[1, 5].Value = "linked_3";
                effectSheet.Cells[1, 6].Value = "linked_4";

                cardSheet.Cells[1, 1].Value = "id";
                cardSheet.Cells[1, 2].Value = "prop_1";
                cardSheet.Cells[1, 3].Value = "val_1";
                cardSheet.Cells[1, 4].Value = "prop_2";
                cardSheet.Cells[1, 5].Value = "val_2";
                cardSheet.Cells[1, 6].Value = "prop_3";
                cardSheet.Cells[1, 7].Value = "val_3";
                cardSheet.Cells[1, 8].Value = "prop_4";
                cardSheet.Cells[1, 9].Value = "val_4";
                cardSheet.Cells[1, 10].Value = "effect";

                int effectID = 1;
                int cardID = 1;

                foreach (Card c in Data.work.cards) 
                { 
                    // cards
                    List<Block> blocks = c.GetOrderedBlocks();
                    cardSheet.Cells[2+cardID, 1].Value = c.id;
                    for (int i = 0; i < c.attrs.Count; i++) 
                    {
                        cardSheet.Cells[2+cardID, 2+i*2].Value = c.attrs[i].name;
                        cardSheet.Cells[2+cardID, 3+i*2].Value = c.attrs[i].value;
                    }
                    cardSheet.Cells[2 + cardID, 10].Value = "id" + effectID;
                    //effects
                    foreach (Block b in blocks) 
                    {
                        effectSheet.Cells[2 + effectID,1].Value = "id"+effectID;
                        effectSheet.Cells[2 + effectID, 2].Value = b.id;
                        for (int i = 0; i < b.operands.Count; i++) 
                        {
                            if (b.operands[i].type == OperandType.Num)
                                effectSheet.Cells[2 + effectID, 3 + i].Value = b.operands[i].num;
                            else
                                effectSheet.Cells[2 + effectID, 3 + i].Value = "id" + (effectID + blocks.IndexOf(b.operands[i].linkedBlock));
                        }
                        effectID++;
                    }
                    cardID++;
                }

                package.Save();
            }

            Debug.Log("Excel Created: " + path);
        }

        private void SaveGitConfig()
        {
            PlayerPrefs.SetString(PrefKeyToken, m_cont.m_txtToken.text);
            PlayerPrefs.SetString(PrefKeyRepo, m_cont.m_txtGitRepo.text);
            PlayerPrefs.Save();
        }

        private void OnClickPull()
        {
            SaveGitConfig();
            string token = m_cont.m_txtToken.text;
            string repoConfig = m_cont.m_txtGitRepo.text;

            m_cont.m_btnPull.touchable = false;
            m_cont.m_btnPull.title = "拉取中...";
            Debug.Log("start pulling");
            Debug.Log(token);
            Debug.Log(repoConfig);
            var test = GitSync.Pull(token, repoConfig, OnPullSuccess, OnPullError);
            CoroutineQueue.inst.Enqueue(test);
        }

        private void OnPullSuccess(string json)
        {
            Debug.Log(json);
            Data.work = JsonUtility.FromJson<Work>(json);
            Data.work.ResolveLinks();
            m_cont.m_txtExport.text = Data.work.export;
            Msg.Dispatch(MsgID.OnDataChanged);

            m_cont.m_btnPull.touchable = true;
            m_cont.m_btnPull.title = pullTitle;
        }

        private void OnPullError(string error)
        {
            Debug.LogError(error);
            m_cont.m_btnPull.touchable = true;
            m_cont.m_btnPull.title = "失败: " + error;
        }

        private void OnClickPush()
        {
            SaveGitConfig();
            string token = m_cont.m_txtToken.text;
            string repoConfig = m_cont.m_txtGitRepo.text;

            Data.work.PrepareForSave();
            string json = JsonUtility.ToJson(Data.work, true);

            m_cont.m_btnPush.touchable = false;
            m_cont.m_btnPush.title = "上传中...";

            CoroutineQueue.inst.Enqueue(GitSync.Push(token, repoConfig, json, OnPushSuccess, OnPushError));
        }

        private void OnPushSuccess()
        {
            m_cont.m_btnPush.touchable = true;
            m_cont.m_btnPush.title = pushTitle;
        }

        private void OnPushError(string error)
        {
            Debug.LogError(error);
            m_cont.m_btnPush.touchable = true;
            m_cont.m_btnPush.title = "失败: " + error;
        }
    }
}
