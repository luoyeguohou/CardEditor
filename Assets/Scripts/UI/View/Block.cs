using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Block : GButton
    {
        Block b;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstOperand.itemRenderer = OperandIR;
            m_txtName.onChanged.Add(OnChangeID);
        }

        public void UpdateView(Block b)
        {
            this.b = b;
            m_txtName.text = b.id;
            m_lstOperand.numItems = b.operands.Count;
            m_opeNum.selectedIndex = b.operands.Count;
        }

        private void OnChangeID()
        {
            b.id = m_txtName.text;
            Debug.Log(m_txtName.text);
            Data.SyncBlockId(b);
        }

        private void OperandIR(int index, GObject g)
        {
            Operand o = b.operands[index];
            UI_Operand ui = (UI_Operand)g;
            ui.SetView(o);
        }

        public Operand GetOperand()
        {
            if (m_lstOperand.selectedIndex < 0) return null;
            return b.operands[m_lstOperand.selectedIndex];
        }

        public int GetSelectIndex()
        {
            return Mathf.Max(m_lstOperand.selectedIndex, 0);
        }

        public Block GetBlockData()
        {
            return b;
        }

        /// <summary>
        /// Returns the actual UI_Operand item views currently in the list.
        /// Assumes m_lstOperand is a non-virtual list (all items realized as children).
        /// </summary>
        public UI_Operand[] GetOperandViews()
        {
            UI_Operand[] arr = new UI_Operand[m_lstOperand.numChildren];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (UI_Operand)m_lstOperand.GetChildAt(i);
            return arr;
        }
    }
}
