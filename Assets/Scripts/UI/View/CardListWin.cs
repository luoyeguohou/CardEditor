using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardListWin : FairyWindow
    {
        private readonly List<Card> filteredCards = new List<Card>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnClose.onClick.Add(Dispose);
            m_cont.m_btnDel.onClick.Add(OnClickDel);
            m_cont.m_btnAdd.onClick.Add(OnClickAdd);
            m_cont.m_btnEdit.onClick.Add(OnClickEdit);
            m_cont.m_lstCard.itemRenderer = CardIR;
            m_cont.m_txtFilter.onChanged.Add(OnFilterChanged);
            m_cont.m_isFiltering.selectedIndex = 0;
            Msg.Bind(MsgID.OnDataChanged,UpdateView);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.OnDataChanged, UpdateView);
        }

        public void UpdateView(object[] p = null)
        {
            string keyword = (m_cont.m_txtFilter.text ?? string.Empty).Trim();
            bool isFiltering = !string.IsNullOrEmpty(keyword);
            m_cont.m_isFiltering.selectedIndex = isFiltering ? 1 : 0;

            filteredCards.Clear();
            foreach (Card card in Data.work.cards)
            {
                if (!isFiltering ||
                    (!string.IsNullOrEmpty(card.id) &&
                     card.id.IndexOf(keyword, System.StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    filteredCards.Add(card);
                }
            }

            m_cont.m_lstCard.numItems = filteredCards.Count;
        }

        private void OnFilterChanged()
        {
            if (string.IsNullOrEmpty(m_cont.m_txtFilter.text))
                m_cont.m_isFiltering.selectedIndex = 0;

            UpdateView();
        }

        private void CardIR(int index, GObject g)
        {
            UI_CardPreview ui = (UI_CardPreview)g;
            Card c = filteredCards[index];
            ui.m_txtID.text = c.id;
            ui.m_txtID.onChanged.Add(() =>
            {
                c.id = ui.m_txtID.text;
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
            Data.work.cards.Remove(filteredCards[index]);
            UpdateView();
        }
        private void OnClickEdit()
        {
            int index = m_cont.m_lstCard.selectedIndex;
            if (index == -1) return;
            Card c = filteredCards[index];
            FGUIUtil.CreateWindow<UI_CardWin>("CardWin").Init(c);
        }
    }
}
