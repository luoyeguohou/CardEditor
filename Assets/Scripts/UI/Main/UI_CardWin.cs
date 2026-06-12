/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_CardCont m_cont;
        public const string URL = "ui://bkt1ky7fbwuy9";

        public static UI_CardWin CreateInstance()
        {
            return (UI_CardWin)UIPackage.CreateObject("Main", "CardWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_CardCont)GetChildAt(1);
        }
    }
}