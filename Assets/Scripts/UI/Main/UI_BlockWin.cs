/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BlockWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_BlockCont m_cont;
        public const string URL = "ui://bkt1ky7fbwuy3";

        public static UI_BlockWin CreateInstance()
        {
            return (UI_BlockWin)UIPackage.CreateObject("Main", "BlockWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_BlockCont)GetChildAt(1);
        }
    }
}