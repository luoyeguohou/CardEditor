/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BlockCont : GComponent
    {
        public GList m_lstBlock;
        public GButton m_btnAdd;
        public GButton m_btnDel;
        public GButton m_btnEdit;
        public GButton m_btnClose;
        public const string URL = "ui://bkt1ky7fotdeh";

        public static UI_BlockCont CreateInstance()
        {
            return (UI_BlockCont)UIPackage.CreateObject("Main", "BlockCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstBlock = (GList)GetChildAt(0);
            m_btnAdd = (GButton)GetChildAt(1);
            m_btnDel = (GButton)GetChildAt(2);
            m_btnEdit = (GButton)GetChildAt(3);
            m_btnClose = (GButton)GetChildAt(4);
        }
    }
}