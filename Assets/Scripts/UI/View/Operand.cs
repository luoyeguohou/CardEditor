using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Operand : GButton
    {
        private Operand o;

        public void SetView(Operand o) {
            this.o = o;
            m_type.selectedIndex = (int)o.type;
        }

        public Operand GetOperandData()
        {
            return o;
        }
    }
}
