using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Prop : GButton
    {
        private Attr attr;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_txtName.onChanged.Add(UpdateAttr);
            m_txtVal.onChanged.Add(UpdateAttr);
        }
        public void SetView(Attr attr)
        {
            this.attr = attr;
            m_txtName.text = attr.name;
            m_txtVal.text = attr.value;
        }
        private void UpdateAttr()
        {
            attr.name = m_txtName.text;
            attr.value = m_txtVal.text;
        }
    }
}
