/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Operand : GButton
    {
        public Controller m_type;
        public GTextInput m_txtOperand;
        public GGraph m_btn;
        public const string URL = "ui://bkt1ky7fbwuye";

        public static UI_Operand CreateInstance()
        {
            return (UI_Operand)UIPackage.CreateObject("Main", "Operand");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_txtOperand = (GTextInput)GetChildAt(2);
            m_btn = (GGraph)GetChildAt(3);
        }
    }
}