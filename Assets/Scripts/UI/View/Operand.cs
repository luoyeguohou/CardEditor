using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Operand : GButton
    {
        private Operand o;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_txtOperand.onChanged.Add(OnChangeText);
        }


        public void SetView(Operand o) {
            this.o = o;
            m_type.selectedIndex = (int)o.type;
            m_txtOperand.text = o.data;
        }

        public Operand GetOperandData()
        {
            return o;
        }

        private void OnChangeText() 
        { 
            o.data = m_txtOperand.text;
        }
    }
}
