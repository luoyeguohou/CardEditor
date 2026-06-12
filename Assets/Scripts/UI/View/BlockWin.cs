using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main {
    public partial class UI_BlockWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnClose.onClick.Add(Dispose);
            m_cont.m_btnAdd.onClick.Add(OnClickAdd);
            m_cont.m_btnDel.onClick.Add(OnClickRemove);
            m_cont.m_btnEdit.onClick.Add(OnClickEdit);
            m_cont.m_lstBlock.itemRenderer = BlockIR;
            Msg.Bind(MsgID.OnDataChanged,InitView);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.OnDataChanged, InitView);
        }

        public void InitView(object[] p = null) {
            m_cont.m_lstBlock.numItems = Data.work.blocks.Count;
        }

        private void OnClickAdd() {
            Data.work.blocks.Insert(Mathf.Max(m_cont.m_lstBlock.selectedIndex,0),Block.NewBlock());
            InitView();
        }

        private void OnClickRemove() {
            Data.work.blocks.RemoveAt(m_cont.m_lstBlock.selectedIndex);
            InitView();
        }

        private void OnClickEdit() {
            if (m_cont.m_lstBlock.selectedIndex == -1) return;
            Block b = Data.work.blocks[m_cont.m_lstBlock.selectedIndex];
            FGUIUtil.CreateWindow<UI_BlockEditWin>("BlockEditWin").InitView(b);
        }

        private void BlockIR(int index, GObject g) { 
            UI_Block ui = (UI_Block)g;
            ui.UpdateView(Data.work.blocks[index]);
        }
    }
}
