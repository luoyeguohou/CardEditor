/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ChooseBlockCont : GComponent
    {
        public GList m_lstBlock;
        public GTextInput m_txtFilter;
        public GButton m_btnConfirm;
        public const string URL = "ui://bkt1ky7fotdek";

        public static UI_ChooseBlockCont CreateInstance()
        {
            return (UI_ChooseBlockCont)UIPackage.CreateObject("Main", "ChooseBlockCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstBlock = (GList)GetChildAt(0);
            m_txtFilter = (GTextInput)GetChildAt(2);
            m_btnConfirm = (GButton)GetChildAt(3);
        }
    }
}