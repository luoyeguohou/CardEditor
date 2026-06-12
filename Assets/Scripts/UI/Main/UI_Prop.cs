/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Prop : GButton
    {
        public GTextInput m_txtName;
        public GTextInput m_txtVal;
        public const string URL = "ui://bkt1ky7fbwuy8";

        public static UI_Prop CreateInstance()
        {
            return (UI_Prop)UIPackage.CreateObject("Main", "Prop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtName = (GTextInput)GetChildAt(3);
            m_txtVal = (GTextInput)GetChildAt(4);
        }
    }
}