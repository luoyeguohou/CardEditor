/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ChooseBlockWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_ChooseBlockCont m_cont;
        public const string URL = "ui://bkt1ky7fbwuya";

        public static UI_ChooseBlockWin CreateInstance()
        {
            return (UI_ChooseBlockWin)UIPackage.CreateObject("Main", "ChooseBlockWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_ChooseBlockCont)GetChildAt(1);
        }
    }
}