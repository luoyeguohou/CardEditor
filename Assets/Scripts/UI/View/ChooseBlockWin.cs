using FairyGUI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Main {
    public partial class UI_ChooseBlockWin : FairyWindow
    {
        private TaskCompletionSource<Block> tcs;
        private List<Block> filtered = new List<Block>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstBlock.itemRenderer = BlockIR;
            m_cont.m_txtFilter.onChanged.Add(OnFilterChanged);
            m_cont.m_btnConfirm.onClick.Add(OnClickConfirm);
        }

        public Task<Block> WaitForResult()
        {
            tcs = new TaskCompletionSource<Block>();
            RefreshList();
            return tcs.Task;
        }

        public override void Dispose()
        {
            tcs?.TrySetResult(null);
            base.Dispose();
        }

        private void RefreshList()
        {
            string keyword = m_cont.m_txtFilter.text.Trim().ToLower();
            filtered.Clear();
            foreach (var b in Data.work.blocks)
            {
                if (string.IsNullOrEmpty(keyword) || b.id.ToLower().Contains(keyword))
                    filtered.Add(b);
            }
            m_cont.m_lstBlock.numItems = filtered.Count;
        }

        private void OnFilterChanged(EventContext context)
        {
            RefreshList();
        }

        private void BlockIR(int index, GObject g)
        {
            UI_Block ui = (UI_Block)g;
            ui.UpdateView(filtered[index]);
        }

        private void OnClickConfirm()
        {
            int idx = m_cont.m_lstBlock.selectedIndex;
            if (idx < 0 || idx >= filtered.Count) return;
            tcs?.TrySetResult(filtered[idx]);
            Dispose();
        }
    }
}
