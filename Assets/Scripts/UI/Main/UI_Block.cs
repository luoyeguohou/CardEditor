/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Block : GButton
    {
        public Controller m_opeNum;
        public GTextInput m_txtName;
        public GList m_lstOperand;
        public const string URL = "ui://bkt1ky7fbwuy2";

        public static UI_Block CreateInstance()
        {
            return (UI_Block)UIPackage.CreateObject("Main", "Block");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_opeNum = GetControllerAt(0);
            m_txtName = (GTextInput)GetChildAt(3);
            m_lstOperand = (GList)GetChildAt(4);
        }
    }
}