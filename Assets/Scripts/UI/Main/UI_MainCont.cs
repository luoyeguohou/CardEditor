/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MainCont : GComponent
    {
        public Controller m_platform;
        public GButton m_btnLoadWork;
        public GButton m_btnSaveWork;
        public GButton m_btnDefBlock;
        public GButton m_btnEditCard;
        public GTextField m_txtExport;
        public GButton m_btnChangePath;
        public GButton m_btnExport;
        public GTextInput m_txtToken;
        public GTextInput m_txtGitRepo;
        public GButton m_btnPull;
        public GButton m_btnPush;
        public const string URL = "ui://bkt1ky7fjflcl";

        public static UI_MainCont CreateInstance()
        {
            return (UI_MainCont)UIPackage.CreateObject("Main", "MainCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_platform = GetControllerAt(0);
            m_btnLoadWork = (GButton)GetChildAt(2);
            m_btnSaveWork = (GButton)GetChildAt(3);
            m_btnDefBlock = (GButton)GetChildAt(4);
            m_btnEditCard = (GButton)GetChildAt(5);
            m_txtExport = (GTextField)GetChildAt(9);
            m_btnChangePath = (GButton)GetChildAt(10);
            m_btnExport = (GButton)GetChildAt(11);
            m_txtToken = (GTextInput)GetChildAt(14);
            m_txtGitRepo = (GTextInput)GetChildAt(17);
            m_btnPull = (GButton)GetChildAt(18);
            m_btnPush = (GButton)GetChildAt(19);
        }
    }
}