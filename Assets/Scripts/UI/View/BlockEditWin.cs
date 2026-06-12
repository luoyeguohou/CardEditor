using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_BlockEditWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnClose.onClick.Add(Dispose);
            m_cont.m_btnChangeType.onClick.Add(OnClickChangeType);
            m_cont.m_btnAddOperand.onClick.Add(OnClickAddOperand);
            m_cont.m_btnDelOperand.onClick.Add(OnClickDeleteOperand);
            m_cont.m_block.m_lstOperand.onClickItem.Add(OnClickOperand);
            m_cont.m_btnMoveUp.onClick.Add(MoveUp);
            m_cont.m_btnMoveDown.onClick.Add(MoveDown);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.Dispatch(MsgID.OnDataChanged);
        }

        Block b;

        public void InitView(Block b)
        {
            this.b = b;
            m_cont.m_block.UpdateView(b);
        }

        private void OnClickOperand()
        {
            Operand operand = m_cont.m_block.GetOperand();
            if (operand == null) return;
            m_cont.m_combType.selectedIndex = (int)operand.type;
        }

        private void OnClickChangeType()
        {
            Operand o = m_cont.m_block.GetOperand();
            if (o == null) return;
            o.type = (OperandType)m_cont.m_combType.selectedIndex;
            m_cont.m_block.UpdateView(b);
            Data.SyncBlockOperands(b);
        }
        private void OnClickAddOperand()
        {
            b.operands.Insert(m_cont.m_block.GetSelectIndex(), new Operand());
            m_cont.m_block.UpdateView(b);
            Data.SyncBlockOperands(b);
        }
        private void OnClickDeleteOperand()
        {
            b.operands.Remove(m_cont.m_block.GetOperand());
            m_cont.m_block.UpdateView(b);
            Data.SyncBlockOperands(b);
        }

        private void MoveUp()
        {
            int index = m_cont.m_block.m_lstOperand.selectedIndex;
            if (index <= 0) return;

            Operand operand = b.operands[index];
            b.operands.RemoveAt(index);
            b.operands.Insert(index - 1, operand);

            m_cont.m_block.UpdateView(b);
            m_cont.m_block.m_lstOperand.selectedIndex = index - 1;
            Data.SyncBlockOperands(b);
        }
        private void MoveDown()
        {
            int index = m_cont.m_block.m_lstOperand.selectedIndex;
            if (index < 0 || index >= b.operands.Count - 1) return;

            Operand operand = b.operands[index];
            b.operands.RemoveAt(index);
            b.operands.Insert(index + 1, operand);

            m_cont.m_block.UpdateView(b);
            m_cont.m_block.m_lstOperand.selectedIndex = index + 1;
            Data.SyncBlockOperands(b);
        }
    }
}
