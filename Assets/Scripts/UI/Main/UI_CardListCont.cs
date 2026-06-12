/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardListCont : GComponent
    {
        public GList m_lstCard;
        public GButton m_btnAdd;
        public GButton m_btnDel;
        public GButton m_btnEdit;
        public GButton m_btnClose;
        public const string URL = "ui://bkt1ky7fotdei";

        public static UI_CardListCont CreateInstance()
        {
            return (UI_CardListCont)UIPackage.CreateObject("Main", "CardListCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstCard = (GList)GetChildAt(0);
            m_btnAdd = (GButton)GetChildAt(1);
            m_btnDel = (GButton)GetChildAt(2);
            m_btnEdit = (GButton)GetChildAt(3);
            m_btnClose = (GButton)GetChildAt(4);
        }
    }
}