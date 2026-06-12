/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BlockEditCont : GComponent
    {
        public UI_Block m_block;
        public GButton m_btnAddOperand;
        public GButton m_btnDelOperand;
        public GComboBox m_combType;
        public GButton m_btnChangeType;
        public GButton m_btnClose;
        public GButton m_btnMoveUp;
        public GButton m_btnMoveDown;
        public const string URL = "ui://bkt1ky7fotdeg";

        public static UI_BlockEditCont CreateInstance()
        {
            return (UI_BlockEditCont)UIPackage.CreateObject("Main", "BlockEditCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_block = (UI_Block)GetChildAt(0);
            m_btnAddOperand = (GButton)GetChildAt(1);
            m_btnDelOperand = (GButton)GetChildAt(2);
            m_combType = (GComboBox)GetChildAt(3);
            m_btnChangeType = (GButton)GetChildAt(4);
            m_btnClose = (GButton)GetChildAt(5);
            m_btnMoveUp = (GButton)GetChildAt(6);
            m_btnMoveDown = (GButton)GetChildAt(7);
        }
    }
}