using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hashing
{
    public sealed class MoonTree : ScrollableControl
    {
        private const int BaseCardGap = 10;
        private const int BaseCardInset = 10;
        private const int BaseCardRadius = 8;
        private const int BaseHeaderHeight = 38;
        private const int BaseChildHeight = 32;

        private readonly List<CardLayout> _cards = new List<CardLayout>();
        private readonly List<NodeLayout> _visibleNodes = new List<NodeLayout>();
        private readonly Dictionary<MoonTreeNode, NodeLayout> _layoutByNode = new Dictionary<MoonTreeNode, NodeLayout>();
        private readonly MoonTreeNodeCollection _nodes;
        private readonly VScrollBar _verticalScrollBar;
        private MoonTreeNode _selectedNode;
        private MoonTreeNode _hoveredNode;
        private Font _headerFont;
        private int _updateCount;
        private int _layoutWidth = -1;
        private int _viewportWidth;
        private int _scrollOffset;
        private bool _layoutDirty = true;
        private bool _rebuildingLayout;
        private System.Windows.Forms.BorderStyle _borderStyle;

        public MoonTree()
        {
            _nodes = new MoonTreeNodeCollection(this, null);
            AutoScroll = false;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
            BackColor = Fluent.Surface;
            ForeColor = Fluent.Text;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable | ControlStyles.Opaque, true);
            UpdateStyles();
            TabStop = true;

            _verticalScrollBar = new VScrollBar
            {
                Visible = false,
                TabStop = false,
                Width = SystemInformation.VerticalScrollBarWidth,
                BackColor = Fluent.Surface,
                ForeColor = Fluent.TextSecondary
            };
            _verticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
            Controls.Add(_verticalScrollBar);
            UpdateHeaderFont();
        }

        public MoonTreeNodeCollection Nodes { get { return _nodes; } }

        public MoonTreeNode SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                MoonTreeNode next = value != null && value.OwnerTree == this ? value : null;
                if (ReferenceEquals(_selectedNode, next))
                    return;

                _selectedNode = next;
                Invalidate();
            }
        }

        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (_borderStyle == value)
                    return;
                _borderStyle = value;
                Invalidate();
            }
        }

        public event EventHandler<MoonTreeNodeMouseEventArgs> NodeMouseClick;
        public event EventHandler<MoonTreeNodeMouseEventArgs> NodeMouseDoubleClick;

        public void BeginUpdate()
        {
            _updateCount++;
        }

        public void EndUpdate()
        {
            if (_updateCount == 0)
                return;

            _updateCount--;
            if (_updateCount == 0 && _layoutDirty)
                RebuildLayout();
        }

        public void ExpandAll()
        {
            BeginUpdate();
            try
            {
                foreach (MoonTreeNode node in _nodes)
                    ExpandRecursively(node);
                RequestLayout();
            }
            finally
            {
                EndUpdate();
            }
        }

        public void CollapseAll()
        {
            BeginUpdate();
            try
            {
                if (_selectedNode != null)
                {
                    MoonTreeNode root = _selectedNode;
                    while (root.Parent != null)
                        root = root.Parent;
                    SelectedNode = root;
                }

                foreach (MoonTreeNode node in _nodes)
                    CollapseRecursively(node);
                RequestLayout();
            }
            finally
            {
                EndUpdate();
            }
        }

        public bool ContainsNode(MoonTreeNode node)
        {
            return node != null && ReferenceEquals(node.OwnerTree, this);
        }

        internal void OnNodeCollectionChanged()
        {
            if (_selectedNode != null && !ContainsNode(_selectedNode))
                _selectedNode = null;
            RequestLayout();
        }

        internal void ResetScrollPosition()
        {
            if (_verticalScrollBar.Value != 0)
                _verticalScrollBar.Value = 0;
            else
            {
                _scrollOffset = 0;
                Invalidate(ClientRectangle);
            }
        }

        internal void OnNodeStateChanged(MoonTreeNode node)
        {
            if (!node.IsExpanded && _selectedNode != null && IsDescendantOf(_selectedNode, node))
                SelectedNode = node;
            RequestLayout();
        }

        internal void EnsureNodeVisible(MoonTreeNode node)
        {
            if (!ContainsNode(node))
                return;

            EnsureLayout();
            NodeLayout layout = FindNodeLayout(node);
            if (layout == null)
                return;

            int next = _scrollOffset;
            if (layout.Bounds.Top < _scrollOffset)
                next = layout.Bounds.Top;
            else if (layout.Bounds.Bottom > _scrollOffset + ClientSize.Height)
                next = layout.Bounds.Bottom - ClientSize.Height;

            SetScrollOffset(next);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RequestLayout();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateHeaderFont();
            RequestLayout();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RequestLayout();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (_nodes != null && ClientSize.Width != _layoutWidth)
                RequestLayout();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            _verticalScrollBar.Width = SystemInformation.VerticalScrollBarWidth;
            RequestLayout();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate(ClientRectangle);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!_verticalScrollBar.Visible || e.Delta == 0)
                return;

            int lines = SystemInformation.MouseWheelScrollLines;
            int amount = lines == -1
                ? ClientSize.Height
                : Math.Max(1, lines) * Fluent.Scale(this, BaseChildHeight);
            int notches = Math.Max(1, Math.Abs(e.Delta) / 120);
            SetScrollOffset(_scrollOffset - Math.Sign(e.Delta) * amount * notches);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            EnsureLayout();

            Graphics graphics = e.Graphics;
            using (SolidBrush background = new SolidBrush(BackColor.IsEmpty ? Fluent.Surface : BackColor))
                graphics.FillRectangle(background, ClientRectangle);

            int contentTop = _scrollOffset;
            int contentBottom = contentTop + ClientSize.Height;
            Rectangle contentViewport = new Rectangle(0, contentTop, _viewportWidth, ClientSize.Height);
            Rectangle paintClip = e.ClipRectangle;
            Point scrollOffset = new Point(0, -contentTop);

            int firstCard = FindFirstVisibleCard(contentTop);
            for (int i = firstCard; i < _cards.Count; i++)
            {
                CardLayout card = _cards[i];
                if (card.Bounds.Top >= contentBottom)
                    break;

                Rectangle clientBounds = Offset(card.Bounds, scrollOffset);
                if (card.Bounds.Bottom > contentTop && card.Bounds.IntersectsWith(contentViewport) &&
                    clientBounds.IntersectsWith(paintClip))
                    DrawCard(graphics, card, scrollOffset);
            }

            if (_borderStyle != System.Windows.Forms.BorderStyle.None)
            {
                using (Pen border = new Pen(Fluent.Border))
                {
                    Rectangle bounds = ClientRectangle;
                    bounds.Width -= 1;
                    bounds.Height -= 1;
                    graphics.DrawRectangle(border, bounds);
                }
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            Point point = ToContentPoint(e.Location);
            NodeLayout hit = HitTest(point);
            if (hit != null && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
            {
                SelectedNode = hit.Node;
                if (e.Button == MouseButtons.Left && e.Clicks == 1 &&
                    hit.IsRoot && hit.GlyphBounds.Contains(point))
                    hit.Node.IsExpanded = !hit.Node.IsExpanded;
            }

            base.OnMouseDown(e);
            if (hit != null)
            {
                EventHandler<MoonTreeNodeMouseEventArgs> handler = NodeMouseClick;
                if (handler != null)
                    handler(this, new MoonTreeNodeMouseEventArgs(hit.Node, e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Point point = ToContentPoint(e.Location);
            NodeLayout hit = HitTest(point);
            if (hit != null)
            {
                if (e.Button == MouseButtons.Left && hit.IsRoot && hit.Node.Nodes.Count > 0 &&
                    !hit.GlyphBounds.Contains(point))
                    hit.Node.IsExpanded = !hit.Node.IsExpanded;

                EventHandler<MoonTreeNodeMouseEventArgs> handler = NodeMouseDoubleClick;
                if (handler != null)
                    handler(this, new MoonTreeNodeMouseEventArgs(hit.Node, e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            NodeLayout hit = HitTest(ToContentPoint(e.Location));
            MoonTreeNode next = hit == null ? null : hit.Node;
            if (!ReferenceEquals(_hoveredNode, next))
            {
                _hoveredNode = next;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hoveredNode = null;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_visibleNodes.Count > 0)
            {
                int current = IndexOfVisibleNode(_selectedNode);
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        SelectVisibleNode(current <= 0 ? 0 : current - 1);
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        SelectVisibleNode(current < 0 ? 0 : Math.Min(_visibleNodes.Count - 1, current + 1));
                        e.Handled = true;
                        break;
                    case Keys.Home:
                        SelectVisibleNode(0);
                        e.Handled = true;
                        break;
                    case Keys.End:
                        SelectVisibleNode(_visibleNodes.Count - 1);
                        e.Handled = true;
                        break;
                    case Keys.PageUp:
                        SetScrollOffset(_scrollOffset - ClientSize.Height);
                        e.Handled = true;
                        break;
                    case Keys.PageDown:
                        SetScrollOffset(_scrollOffset + ClientSize.Height);
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        if (_selectedNode != null && _selectedNode.Nodes.Count > 0)
                        {
                            if (!_selectedNode.IsExpanded)
                                _selectedNode.IsExpanded = true;
                            else if (_selectedNode.Nodes.Count > 0)
                                SelectedNode = _selectedNode.Nodes[0];
                        }
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        if (_selectedNode != null && _selectedNode.Parent != null)
                            SelectedNode = _selectedNode.Parent;
                        else if (_selectedNode != null && _selectedNode.IsExpanded)
                            _selectedNode.IsExpanded = false;
                        e.Handled = true;
                        break;
                    case Keys.Enter:
                        if (_selectedNode != null && _selectedNode.Nodes.Count > 0)
                            _selectedNode.IsExpanded = !_selectedNode.IsExpanded;
                        e.Handled = true;
                        break;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _headerFont != null)
            {
                _headerFont.Dispose();
                _headerFont = null;
            }
            base.Dispose(disposing);
        }

        private void DrawCard(Graphics graphics, CardLayout card, Point scrollOffset)
        {
            GraphicsState graphicsState = graphics.Save();
            Rectangle cardBounds = Offset(card.Bounds, scrollOffset);
            Rectangle headerBounds = Offset(card.HeaderBounds, scrollOffset);
            Rectangle glyphBounds = Offset(card.GlyphBounds, scrollOffset);
            try
            {
                graphics.SetClip(cardBounds, CombineMode.Intersect);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color borderColor = IsWithin(card.Root, _selectedNode) ? Fluent.Accent : Fluent.Border;
                Color fillColor = ReferenceEquals(_hoveredNode, card.Root) ? Fluent.ControlHover : Fluent.Control;

                using (GraphicsPath path = Fluent.RoundedRect(
                    new RectangleF(cardBounds.X + 0.5f, cardBounds.Y + 0.5f,
                        cardBounds.Width - 1f, cardBounds.Height - 1f),
                    Fluent.Scale(this, BaseCardRadius)))
                using (SolidBrush fill = new SolidBrush(fillColor))
                using (Pen border = new Pen(borderColor, IsWithin(card.Root, _selectedNode) ? 1.5f : 1f))
                {
                    graphics.FillPath(fill, path);
                    graphics.DrawPath(border, path);
                }

                if (card.ChildRows.Count > 0)
                {
                    int dividerY = headerBounds.Bottom + Fluent.Scale(this, 1);
                    using (Pen divider = new Pen(Fluent.Border))
                    {
                        int inset = Fluent.Scale(this, 12);
                        graphics.DrawLine(divider, cardBounds.Left + inset, dividerY,
                            cardBounds.Right - inset, dividerY);
                    }
                }

                DrawNodeRow(graphics, card.Root, headerBounds, glyphBounds, true);
                foreach (NodeLayout child in card.ChildRows)
                    DrawNodeRow(graphics, child.Node, Offset(child.Bounds, scrollOffset), Rectangle.Empty, false);
            }
            finally
            {
                graphics.Restore(graphicsState);
            }
        }

        private static Rectangle Offset(Rectangle bounds, Point offset)
        {
            bounds.Offset(offset);
            return bounds;
        }

        private void DrawNodeRow(Graphics graphics, MoonTreeNode node, Rectangle bounds,
            Rectangle glyphBounds, bool isRoot)
        {
            if (ReferenceEquals(node, _selectedNode))
            {
                Rectangle selection = bounds;
                selection.Inflate(-Fluent.Scale(this, isRoot ? 5 : 4), -Fluent.Scale(this, 2));
                using (GraphicsPath path = Fluent.RoundedRect(new RectangleF(selection.X, selection.Y, selection.Width, selection.Height), Fluent.Scale(this, 5)))
                using (SolidBrush brush = new SolidBrush(Fluent.SelectionColor))
                    graphics.FillPath(brush, path);
            }
            else if (ReferenceEquals(node, _hoveredNode) && !isRoot)
            {
                Rectangle hover = bounds;
                hover.Inflate(-Fluent.Scale(this, 5), -Fluent.Scale(this, 2));
                using (GraphicsPath path = Fluent.RoundedRect(new RectangleF(hover.X, hover.Y, hover.Width, hover.Height), Fluent.Scale(this, 5)))
                using (SolidBrush brush = new SolidBrush(Fluent.Blend(Fluent.Control, Fluent.ControlHover, 0.75f)))
                    graphics.FillPath(brush, path);
            }

            int textLeft = bounds.Left + Fluent.Scale(this, isRoot ? 38 : 22);
            int right = bounds.Right - Fluent.Scale(this, 12);
            Rectangle textBounds = Rectangle.FromLTRB(textLeft, bounds.Top, Math.Max(textLeft, right), bounds.Bottom);
            Color color = ReferenceEquals(node, _selectedNode)
                ? Fluent.Text
                : isRoot ? Fluent.AccentText
                : ((node.Tag is DuplicateGroupInfo || node.Tag is DuplicateHashMatch) ? Fluent.TextSecondary :
                    (node.ForeColor.IsEmpty ? Fluent.Text : node.ForeColor));
            Font font = isRoot ? (_headerFont ?? Font) : Font;

            Fluent.DrawText(graphics, node.Text, font, textBounds, color,
                TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix |
                TextFormatFlags.NoPadding | TextFormatFlags.PreserveGraphicsClipping);

            if (isRoot && node.Nodes.Count > 0)
                DrawExpandGlyph(graphics, node, glyphBounds);
        }

        private void DrawExpandGlyph(Graphics graphics, MoonTreeNode node, Rectangle bounds)
        {
            if (bounds.IsEmpty)
                return;

            float centerX = bounds.Left + bounds.Width / 2f;
            float centerY = bounds.Top + bounds.Height / 2f;
            int offset = Fluent.Scale(this, 3);
            using (Pen pen = new Pen(Fluent.TextSecondary, Math.Max(1f, Fluent.Scale(this, 1) * 1.5f)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                if (node.IsExpanded)
                {
                    graphics.DrawLine(pen, centerX - offset, centerY - 1, centerX, centerY + offset - 1);
                    graphics.DrawLine(pen, centerX, centerY + offset - 1, centerX + offset, centerY - 1);
                }
                else
                {
                    graphics.DrawLine(pen, centerX - 1, centerY - offset, centerX + offset, centerY);
                    graphics.DrawLine(pen, centerX + offset, centerY, centerX - 1, centerY + offset);
                }
            }
        }

        private void RequestLayout()
        {
            _layoutDirty = true;
            if (_updateCount == 0 && IsHandleCreated && !_rebuildingLayout)
                RebuildLayout();
            else
                Invalidate();
        }

        private void EnsureLayout()
        {
            if (_layoutDirty && _updateCount == 0)
                RebuildLayout();
        }

        private void RebuildLayout()
        {
            if (_updateCount > 0 || _rebuildingLayout)
            {
                _layoutDirty = true;
                return;
            }

            _rebuildingLayout = true;
            _layoutDirty = false;
            try
            {
                _cards.Clear();
                _visibleNodes.Clear();
                _layoutByNode.Clear();

                int cardInset = Fluent.Scale(this, BaseCardInset);
                int cardGap = Fluent.Scale(this, BaseCardGap);
                int headerHeight = Fluent.Scale(this, BaseHeaderHeight);
                int childHeight = Fluent.Scale(this, BaseChildHeight);
                int contentHeight = CalculateContentHeight(cardGap, headerHeight, childHeight);
                int previousScroll = _scrollOffset;
                bool needsScrollBar = contentHeight > ClientSize.Height;
                int scrollBarWidth = Math.Max(1, SystemInformation.VerticalScrollBarWidth);

                if (_verticalScrollBar.Width != scrollBarWidth)
                    _verticalScrollBar.Width = scrollBarWidth;
                if (_verticalScrollBar.Visible != needsScrollBar)
                    _verticalScrollBar.Visible = needsScrollBar;

                _viewportWidth = Math.Max(0, ClientSize.Width - (needsScrollBar ? scrollBarWidth : 0));
                _verticalScrollBar.SetBounds(_viewportWidth, 0, scrollBarWidth, ClientSize.Height);
                _verticalScrollBar.Minimum = 0;
                _verticalScrollBar.Maximum = Math.Max(0, contentHeight - 1);
                _verticalScrollBar.SmallChange = Math.Max(1, childHeight);
                _verticalScrollBar.LargeChange = Math.Max(1, ClientSize.Height);

                int maxScroll = Math.Max(0, _verticalScrollBar.Maximum - _verticalScrollBar.LargeChange + 1);
                int desiredScroll = Math.Min(Math.Max(0, previousScroll), maxScroll);
                if (_verticalScrollBar.Value != desiredScroll)
                    _verticalScrollBar.Value = desiredScroll;
                _scrollOffset = _verticalScrollBar.Value;

                int cardWidth = Math.Max(0, _viewportWidth - cardInset * 2);
                int top = cardGap;

                foreach (MoonTreeNode root in _nodes)
                {
                    int childCount = root.IsExpanded ? root.Nodes.Count : 0;
                    int childrenHeight = childCount == 0 ? 0 : Fluent.Scale(this, 1) + childCount * childHeight;
                    int cardHeight = Fluent.Scale(this, 8) + headerHeight + childrenHeight + Fluent.Scale(this, 8);
                    Rectangle cardBounds = new Rectangle(cardInset, top, cardWidth, cardHeight);
                    Rectangle headerBounds = new Rectangle(cardBounds.Left + Fluent.Scale(this, 6),
                        cardBounds.Top + Fluent.Scale(this, 4), Math.Max(0, cardBounds.Width - Fluent.Scale(this, 12)), headerHeight);
                    int glyphSize = Fluent.Scale(this, 16);
                    Rectangle glyphBounds = new Rectangle(headerBounds.Left + Fluent.Scale(this, 8),
                        headerBounds.Top + (headerBounds.Height - glyphSize) / 2, glyphSize, glyphSize);

                    CardLayout card = new CardLayout(root, cardBounds, headerBounds, glyphBounds);
                    _cards.Add(card);
                    NodeLayout rootLayout = new NodeLayout(root, headerBounds, glyphBounds, true, _visibleNodes.Count);
                    _visibleNodes.Add(rootLayout);
                    _layoutByNode.Add(root, rootLayout);

                    int childTop = headerBounds.Bottom + Fluent.Scale(this, 1);
                    if (root.IsExpanded)
                    {
                        foreach (MoonTreeNode child in root.Nodes)
                        {
                            Rectangle childBounds = new Rectangle(cardBounds.Left + Fluent.Scale(this, 8),
                                childTop, Math.Max(0, cardBounds.Width - Fluent.Scale(this, 16)), childHeight);
                            NodeLayout childLayout = new NodeLayout(child, childBounds, Rectangle.Empty, false, _visibleNodes.Count);
                            card.ChildRows.Add(childLayout);
                            _visibleNodes.Add(childLayout);
                            _layoutByNode.Add(child, childLayout);
                            childTop += childHeight;
                        }
                    }

                    top += cardHeight + cardGap;
                }

                _layoutWidth = ClientSize.Width;
            }
            finally
            {
                _rebuildingLayout = false;
            }

            if (_layoutDirty && _updateCount == 0)
                RebuildLayout();
            else
                Invalidate(ClientRectangle);
        }

        private Point ToContentPoint(Point point)
        {
            return new Point(point.X, point.Y + _scrollOffset);
        }

        private void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _scrollOffset = _verticalScrollBar.Value;
            Invalidate(ClientRectangle);
        }

        private void SetScrollOffset(int value)
        {
            int maxScroll = Math.Max(0, _verticalScrollBar.Maximum - _verticalScrollBar.LargeChange + 1);
            int next = Math.Max(0, Math.Min(value, maxScroll));
            if (_verticalScrollBar.Value != next)
                _verticalScrollBar.Value = next;
            else
            {
                _scrollOffset = next;
                Invalidate(ClientRectangle);
            }
        }

        private int CalculateContentHeight(int cardGap, int headerHeight, int childHeight)
        {
            int top = cardGap;
            foreach (MoonTreeNode root in _nodes)
            {
                int childCount = root.IsExpanded ? root.Nodes.Count : 0;
                int childrenHeight = childCount == 0 ? 0 : Fluent.Scale(this, 1) + childCount * childHeight;
                int cardHeight = Fluent.Scale(this, 8) + headerHeight + childrenHeight + Fluent.Scale(this, 8);
                top += cardHeight + cardGap;
            }
            return Math.Max(0, top);
        }

        private NodeLayout HitTest(Point contentPoint)
        {
            int low = 0;
            int high = _cards.Count - 1;
            while (low <= high)
            {
                int middle = low + ((high - low) >> 1);
                CardLayout card = _cards[middle];
                if (contentPoint.Y < card.Bounds.Top)
                    high = middle - 1;
                else if (contentPoint.Y >= card.Bounds.Bottom)
                    low = middle + 1;
                else
                {
                    if (!card.Bounds.Contains(contentPoint))
                        return null;
                    if (card.HeaderBounds.Contains(contentPoint))
                        return _layoutByNode[card.Root];
                    foreach (NodeLayout child in card.ChildRows)
                    {
                        if (child.Bounds.Contains(contentPoint))
                            return child;
                    }
                    return null;
                }
            }
            return null;
        }

        private NodeLayout FindNodeLayout(MoonTreeNode node)
        {
            NodeLayout layout;
            return node != null && _layoutByNode.TryGetValue(node, out layout) ? layout : null;
        }

        private int IndexOfVisibleNode(MoonTreeNode node)
        {
            NodeLayout layout = FindNodeLayout(node);
            return layout == null ? -1 : layout.VisibleIndex;
        }

        private int FindFirstVisibleCard(int top)
        {
            int low = 0;
            int high = _cards.Count;
            while (low < high)
            {
                int middle = low + ((high - low) >> 1);
                if (_cards[middle].Bounds.Bottom < top)
                    low = middle + 1;
                else
                    high = middle;
            }
            return low;
        }

        private void SelectVisibleNode(int index)
        {
            if (index < 0 || index >= _visibleNodes.Count)
                return;
            SelectedNode = _visibleNodes[index].Node;
            EnsureNodeVisible(SelectedNode);
        }

        private void UpdateHeaderFont()
        {
            if (_headerFont != null)
                _headerFont.Dispose();
            _headerFont = Font == null ? null : new Font(Font, FontStyle.Bold);
        }

        private static bool IsWithin(MoonTreeNode root, MoonTreeNode node)
        {
            for (MoonTreeNode current = node; current != null; current = current.Parent)
            {
                if (ReferenceEquals(current, root))
                    return true;
            }
            return false;
        }

        private static bool IsDescendantOf(MoonTreeNode node, MoonTreeNode possibleAncestor)
        {
            for (MoonTreeNode current = node.Parent; current != null; current = current.Parent)
            {
                if (ReferenceEquals(current, possibleAncestor))
                    return true;
            }
            return false;
        }

        private static void ExpandRecursively(MoonTreeNode node)
        {
            node.SetExpanded(true);
            foreach (MoonTreeNode child in node.Nodes)
                ExpandRecursively(child);
        }

        private static void CollapseRecursively(MoonTreeNode node)
        {
            node.SetExpanded(false);
            foreach (MoonTreeNode child in node.Nodes)
                CollapseRecursively(child);
        }

        private sealed class CardLayout
        {
            internal readonly MoonTreeNode Root;
            internal readonly Rectangle Bounds;
            internal readonly Rectangle HeaderBounds;
            internal readonly Rectangle GlyphBounds;
            internal readonly List<NodeLayout> ChildRows = new List<NodeLayout>();

            internal CardLayout(MoonTreeNode root, Rectangle bounds, Rectangle headerBounds, Rectangle glyphBounds)
            {
                Root = root;
                Bounds = bounds;
                HeaderBounds = headerBounds;
                GlyphBounds = glyphBounds;
            }
        }

        private sealed class NodeLayout
        {
            internal readonly MoonTreeNode Node;
            internal readonly Rectangle Bounds;
            internal readonly Rectangle GlyphBounds;
            internal readonly bool IsRoot;
            internal readonly int VisibleIndex;

            internal NodeLayout(MoonTreeNode node, Rectangle bounds, Rectangle glyphBounds, bool isRoot, int visibleIndex)
            {
                Node = node;
                Bounds = bounds;
                GlyphBounds = glyphBounds;
                IsRoot = isRoot;
                VisibleIndex = visibleIndex;
            }
        }
    }

    public sealed class MoonTreeNode
    {
        private bool _expanded;
        private string _text;
        private Color _foreColor;
        private MoonTree _ownerTree;
        private MoonTreeNode _parent;
        private MoonTreeNodeCollection _collection;

        public MoonTreeNode(string text)
        {
            _text = text ?? string.Empty;
            _foreColor = Color.Empty;
            Nodes = new MoonTreeNodeCollection(null, this);
        }

        public string Text
        {
            get { return _text; }
            set
            {
                string next = value ?? string.Empty;
                if (string.Equals(_text, next, StringComparison.Ordinal))
                    return;
                _text = next;
                if (_ownerTree != null)
                    _ownerTree.Invalidate();
            }
        }

        public object Tag { get; set; }
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (_foreColor == value)
                    return;
                _foreColor = value;
                if (_ownerTree != null)
                    _ownerTree.Invalidate();
            }
        }
        public MoonTreeNode Parent { get { return _parent; } }
        public MoonTreeNodeCollection Nodes { get; private set; }
        public bool IsExpanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded == value || Nodes.Count == 0)
                    return;
                _expanded = value;
                if (_ownerTree != null)
                    _ownerTree.OnNodeStateChanged(this);
            }
        }

        internal MoonTree OwnerTree { get { return _ownerTree; } }

        public void Expand()
        {
            IsExpanded = true;
        }

        public void Collapse()
        {
            IsExpanded = false;
        }

        public void EnsureVisible()
        {
            if (_ownerTree != null)
                _ownerTree.EnsureNodeVisible(this);
        }

        internal void SetExpanded(bool expanded)
        {
            if (Nodes.Count > 0)
                _expanded = expanded;
        }

        internal void Attach(MoonTree tree, MoonTreeNode parent, MoonTreeNodeCollection collection)
        {
            _ownerTree = tree;
            _parent = parent;
            _collection = collection;
            foreach (MoonTreeNode child in Nodes)
                child.Attach(tree, this, Nodes);
        }

        internal void SetOwner(MoonTree tree)
        {
            _ownerTree = tree;
            foreach (MoonTreeNode child in Nodes)
                child.SetOwner(tree);
        }

        internal MoonTreeNodeCollection Collection { get { return _collection; } }
    }

    public sealed class MoonTreeNodeCollection : IEnumerable<MoonTreeNode>
    {
        private readonly List<MoonTreeNode> _items = new List<MoonTreeNode>();
        private readonly MoonTree _ownerTree;
        private readonly MoonTreeNode _parent;

        internal MoonTreeNodeCollection(MoonTree ownerTree, MoonTreeNode parent)
        {
            _ownerTree = ownerTree;
            _parent = parent;
        }

        public int Count { get { return _items.Count; } }
        public MoonTreeNode this[int index] { get { return _items[index]; } }

        public MoonTreeNode Add(string text)
        {
            MoonTreeNode node = new MoonTreeNode(text);
            Add(node);
            return node;
        }

        public MoonTreeNode Add(MoonTreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            for (MoonTreeNode ancestor = _parent; ancestor != null; ancestor = ancestor.Parent)
            {
                if (ReferenceEquals(ancestor, node))
                    throw new InvalidOperationException("A node cannot be inserted into its own descendants.");
            }

            if (node.Collection != null)
                node.Collection.Remove(node);

            _items.Add(node);
            node.Attach(GetOwnerTree(), _parent, this);
            NotifyChanged();
            return node;
        }

        public void AddRange(MoonTreeNode[] nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException("nodes");

            MoonTree owner = GetOwnerTree();
            if (owner != null)
                owner.BeginUpdate();
            try
            {
                foreach (MoonTreeNode node in nodes)
                    Add(node);
            }
            finally
            {
                if (owner != null)
                    owner.EndUpdate();
            }
        }

        public bool Remove(MoonTreeNode node)
        {
            if (node == null || !_items.Remove(node))
                return false;

            node.Attach(null, null, null);
            NotifyChanged();
            return true;
        }

        public void Clear()
        {
            if (_items.Count == 0)
                return;

            foreach (MoonTreeNode node in _items)
                node.Attach(null, null, null);
            _items.Clear();

            if (_parent == null && _ownerTree != null)
                _ownerTree.ResetScrollPosition();
            NotifyChanged();
        }

        public IEnumerator<MoonTreeNode> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private MoonTree GetOwnerTree()
        {
            return _parent == null ? _ownerTree : _parent.OwnerTree;
        }

        private void NotifyChanged()
        {
            MoonTree owner = GetOwnerTree();
            if (owner != null)
                owner.OnNodeCollectionChanged();
        }
    }

    public sealed class MoonTreeNodeMouseEventArgs : MouseEventArgs
    {
        public MoonTreeNode Node { get; private set; }

        public MoonTreeNodeMouseEventArgs(MoonTreeNode node, MouseButtons button, int clicks,
            int x, int y, int delta)
            : base(button, clicks, x, y, delta)
        {
            Node = node;
        }
    }
}
