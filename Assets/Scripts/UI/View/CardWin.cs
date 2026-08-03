using FairyGUI;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardWin : FairyWindow
    {
        private Card c;
        private List<UI_Block> blockViews = new List<UI_Block>();
        private UI_Block selectedBlockView;

        // operand -> block link lines currently drawn
        private List<GObject> linkLines = new List<GObject>();

        // state while dragging a new link out of an operand's button
        private GObject ghostLine;
        private UI_Operand draggingOperandUI;
        private Operand draggingOperand;
        private Block draggingOwnerBlock;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstProp.itemRenderer = PropIR;
            m_cont.m_btnAddBlock.onClick.Add(OnClickAddBlock);
            m_cont.m_btnAddProp.onClick.Add(OnClickAddProp);
            m_cont.m_btnClose.onClick.Add(Dispose);
            m_cont.m_txtID.onChanged.Add(OnClickConfirmID);
            m_cont.m_btnRemoveBlock.onClick.Add(OnClickRemoveBlock);
            m_cont.m_btnDelProp.onClick.Add(OnClickDelProp);
            m_cont.m_btnMoveUp.onClick.Add(MoveUp);
            m_cont.m_btnMoveDown.onClick.Add(MoveDown);
        }
        public override void Dispose()
        {
            Stage.inst.onTouchMove.Remove(OnLinkDragMove);
            Stage.inst.onTouchEnd.Remove(OnLinkDragEnd);
            base.Dispose();
            Msg.Dispatch(MsgID.OnDataChanged);
        }
        public void Init(Card c)
        {
            this.c = c;
            UpdateView();
            UpdateBlockView();
        }
        private void UpdateView() {
            m_cont.m_lstProp.numItems = c.attrs.Count;
            m_cont.m_txtID.text = c.id;
        }
        private void UpdateBlockView()
        {
            foreach (var bv in blockViews)
                bv.Dispose();
            blockViews.Clear();
            selectedBlockView = null;

            foreach (var block in c.blocks)
            {
                UI_Block ui = UI_Block.CreateInstance();
                ui.UpdateView(block);
                ui.draggable = true;
                ui.SetXY(block.x, block.y);
                ui.onClick.Add(() => OnClickBlockView(ui));
                ui.onDragEnd.Add(() => OnDragEndBlockView(ui, block));
                ui.onDragMove.Add(() => RefreshLines());
                m_cont.AddChild(ui);
                blockViews.Add(ui);

                var operandViews = ui.GetOperandViews();
                for (int j = 0; j < block.operands.Count && j < operandViews.Length; j++)
                {
                    SetupOperandLink(operandViews[j], block.operands[j], block);
                }
            }

            CoroutineQueue.inst.NextFrame(() =>
            {
                RefreshLines();
            });
        }
        private void OnClickBlockView(UI_Block ui)
        {
            selectedBlockView = ui;
        }
        private void OnDragEndBlockView(UI_Block ui, Block block)
        {
            block.x = ui.x;
            block.y = ui.y;
            RefreshLines();
        }
        // ----- dragging a link out of an operand's button -----
        private void SetupOperandLink(UI_Operand opUI, Operand operand, Block owner)
        {
            opUI.m_btn.onTouchBegin.Add((EventContext context) => OnLinkDragStart(opUI, operand, owner, context));
        }
        private void OnLinkDragStart(UI_Operand opUI, Operand operand, Block owner, EventContext context)
        {
            // prevent this touch from bubbling up and triggering the parent
            // UI_Block's own drag-to-move behavior
            context.StopPropagation();
            context.CaptureTouch();

            draggingOperandUI = opUI;
            draggingOperand = operand;
            draggingOwnerBlock = owner;

            ghostLine = UIPackage.CreateObject("Main", "Line");
            m_cont.AddChild(ghostLine);

            Stage.inst.onTouchMove.Add(OnLinkDragMove);
            Stage.inst.onTouchEnd.Add(OnLinkDragEnd);

            OnLinkDragMove(context);
        }
        private void OnLinkDragMove(EventContext context)
        {
            if (ghostLine == null || draggingOperandUI == null) return;
            Vector2 from = m_cont.GlobalToLocal(GetOperandAnchor(draggingOperandUI));
            Vector2 to = m_cont.GlobalToLocal(new Vector2(context.inputEvent.x, context.inputEvent.y));
            FGUIUtil.SetLine(ghostLine, from, to);
        }
        private void OnLinkDragEnd(EventContext context)
        {
            Stage.inst.onTouchMove.Remove(OnLinkDragMove);
            Stage.inst.onTouchEnd.Remove(OnLinkDragEnd);
            if (ghostLine != null)
            {
                ghostLine.RemoveFromParent();
                ghostLine.Dispose();
                ghostLine = null;
            }
            Operand operand = draggingOperand;
            Block owner = draggingOwnerBlock;
            draggingOperandUI = null;
            draggingOperand = null;
            draggingOwnerBlock = null;
            if (operand == null) return;

            Vector2 dropPos = new Vector2(context.inputEvent.x, context.inputEvent.y);
            foreach (var ui in blockViews)
            {
                if (FGUIUtil.HitTest(ui, dropPos))
                {
                    Block target = ui.GetBlockData();
                    if (target == null || target == owner) break; // can't link to itself

                    if (operand.linkedBlock == target)
                        operand.linkedBlock = null; // already linked -> unlink
                    else
                        operand.linkedBlock = target;

                    RefreshLines();
                    break;
                }
            }
        }

        private Vector2 GetOperandAnchor(UI_Operand opUI)
        {
            return opUI.m_btn.LocalToGlobal(new Vector2());
        }

        // ----- persistent link lines -----

        private void RefreshLines()
        {
            foreach (var l in linkLines)
            {
                l.RemoveFromParent();
                l.Dispose();
            }
            linkLines.Clear();

            for (int i = 0; i < c.blocks.Count; i++)
            {
                Block block = c.blocks[i];
                UI_Block blockUI = blockViews[i];
                var operandViews = blockUI.GetOperandViews();

                for (int j = 0; j < block.operands.Count && j < operandViews.Length; j++)
                {
                    Operand operand = block.operands[j];
                    Block linked = operand.linkedBlock;
                    if (linked == null) continue;

                    int targetIdx = c.blocks.IndexOf(linked);
                    if (targetIdx < 0) continue;
                    UI_Block targetUI = blockViews[targetIdx];

                    Vector2 from = m_cont.GlobalToLocal(GetOperandAnchor(operandViews[j]));
                    Vector2 to = m_cont.GlobalToLocal(targetUI.LocalToGlobal(new Vector2(0, targetUI.height / 2)));

                    GObject line = UIPackage.CreateObject("Main", "Line");
                    m_cont.AddChild(line);
                    FGUIUtil.SetLine(line, from, to);
                    linkLines.Add(line);
                }
            }
        }

        private void PropIR(int index, GObject g) {
            UI_Prop ui = (UI_Prop)g;
            Attr attr = c.attrs[index];
            ui.SetView(attr);
        }

        private void OnClickConfirmID()
        {
            c.id = m_cont.m_txtID.text;
        }
        private void OnClickAddProp()
        {
            c.attrs.Add(new Attr());
            UpdateView();
        }
        private void OnClickDelProp()
        {
            if (m_cont.m_lstProp.selectedIndex == -1) return;
            c.attrs.RemoveAt(m_cont.m_lstProp.selectedIndex);
            UpdateView();
        }
        private async void OnClickAddBlock()
        {
            Block b = await FGUIUtil.CreateWindow<UI_ChooseBlockWin>("ChooseBlockWin").WaitForResult();
            if (b == null) return;
            c.blocks.Add(b.Copy());
            UpdateBlockView();
        }
        private void OnClickRemoveBlock()
        {
            if (selectedBlockView == null) return;
            int idx = blockViews.IndexOf(selectedBlockView);
            if (idx < 0) return;
            Block removed = c.blocks[idx];
            c.blocks.RemoveAt(idx);

            // drop dangling links pointing at the removed block
            foreach (var block in c.blocks)
                foreach (var operand in block.operands)
                    if (operand.linkedBlock == removed)
                        operand.linkedBlock = null;

            UpdateBlockView();
        }

        private void MoveUp()
        {
            int index = m_cont.m_lstProp.selectedIndex;
            if (index <= 0) return;

            Attr attr = c.attrs[index];
            c.attrs.RemoveAt(index);
            c.attrs.Insert(index - 1, attr);

            UpdateView();
            m_cont.m_lstProp.selectedIndex = index - 1;
        }
        private void MoveDown()
        {
            int index = m_cont.m_lstProp.selectedIndex;
            if (index < 0 || index >= c.attrs.Count - 1) return;

            Attr attr = c.attrs[index];
            c.attrs.RemoveAt(index);
            c.attrs.Insert(index + 1, attr);

            UpdateView();
            m_cont.m_lstProp.selectedIndex = index + 1;
        }
    }
}
