/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardPreview : GButton
    {
        public GTextInput m_txtID;
        public const string URL = "ui://bkt1ky7fbwuyb";

        public static UI_CardPreview CreateInstance()
        {
            return (UI_CardPreview)UIPackage.CreateObject("Main", "CardPreview");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtID = (GTextInput)GetChildAt(2);
        }
    }
}