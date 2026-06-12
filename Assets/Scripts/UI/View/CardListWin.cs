using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardListWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnClose.onClick.Add(Dispose);
            m_cont.m_btnDel.onClick.Add(OnClickDel);
            m_cont.m_btnAdd.onClick.Add(OnClickAdd);
            m_cont.m_btnEdit.onClick.Add(OnClickEdit);
            m_cont.m_lstCard.itemRenderer = CardIR;
            Msg.Bind(MsgID.OnDataChanged,UpdateView);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.OnDataChanged, UpdateView);
        }

        public void UpdateView(object[] p = null)
        {
            m_cont.m_lstCard.numItems = Data.work.cards.Count;
        }

        private void CardIR(int index, GObject g)
        {
            UI_CardPreview ui = (UI_CardPreview)g;
            Card c = Data.work.cards[index];
            ui.m_txtID.text = c.id;
            ui.m_txtID.onChanged.Add(() =>
            {
                Data.work.cards[index].id = ui.m_txtID.text;
            });
        }
        private void OnClickAdd()
        {
            Data.work.cards.Add(Card.NewCard());
            UpdateView();
        }
        private void OnClickDel()
        {
            int index = m_cont.m_lstCard.selectedIndex;
            if (index == -1) return;
            Data.work.cards.RemoveAt(index);
            UpdateView();
        }
        private void OnClickEdit()
        {
            int index = m_cont.m_lstCard.selectedIndex;
            if (index == -1) return;
            Card c = Data.work.cards[index];
            FGUIUtil.CreateWindow<UI_CardWin>("CardWin").Init(c);
        }
    }
}
