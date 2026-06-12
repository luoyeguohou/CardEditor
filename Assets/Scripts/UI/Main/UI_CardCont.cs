/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardCont : GComponent
    {
        public GList m_lstProp;
        public GButton m_btnAddProp;
        public GButton m_btnDelProp;
        public GButton m_btnAddBlock;
        public GButton m_btnRemoveBlock;
        public GTextInput m_txtID;
        public GButton m_btnClose;
        public GButton m_btnMoveDown;
        public GButton m_btnMoveUp;
        public const string URL = "ui://bkt1ky7fotdej";

        public static UI_CardCont CreateInstance()
        {
            return (UI_CardCont)UIPackage.CreateObject("Main", "CardCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstProp = (GList)GetChildAt(0);
            m_btnAddProp = (GButton)GetChildAt(1);
            m_btnDelProp = (GButton)GetChildAt(2);
            m_btnAddBlock = (GButton)GetChildAt(3);
            m_btnRemoveBlock = (GButton)GetChildAt(4);
            m_txtID = (GTextInput)GetChildAt(6);
            m_btnClose = (GButton)GetChildAt(7);
            m_btnMoveDown = (GButton)GetChildAt(8);
            m_btnMoveUp = (GButton)GetChildAt(9);
        }
    }
}