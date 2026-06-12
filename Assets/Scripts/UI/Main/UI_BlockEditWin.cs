/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BlockEditWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_BlockEditCont m_cont;
        public const string URL = "ui://bkt1ky7fbwuy7";

        public static UI_BlockEditWin CreateInstance()
        {
            return (UI_BlockEditWin)UIPackage.CreateObject("Main", "BlockEditWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_BlockEditCont)GetChildAt(1);
        }
    }
}