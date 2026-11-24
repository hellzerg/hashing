using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    public sealed class MoonSelect : ComboBox
    {
        private const int BaseHeight = 36;
        private const int BaseItemHeight = 32;
        private const int BaseDropDownHeight = 240;

        public MoonSelect()
        {
            FlatStyle = FlatStyle.Flat;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            IntegralHeight = false;
            Font = Fluent.Ui(9.5f);
            BackColor = Fluent.Control;
            ForeColor = Fluent.Text;
            Cursor = Cursors.Hand;
            Height = Fluent.Scale(this, BaseHeight);
            UpdateMetrics();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateMetrics();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateMetrics();
        }

        private void UpdateMetrics()
        {
            ItemHeight = Fluent.Scale(this, BaseItemHeight);
            DropDownHeight = Fluent.Scale(this, BaseDropDownHeight);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;

            bool editArea = (e.State & DrawItemState.ComboBoxEdit) != 0;
            bool selected = (e.State & DrawItemState.Selected) != 0 && !editArea;
            Color background = editArea ? Fluent.Control : selected ? Fluent.SelectionColor : Fluent.Menu;
            Color foreground = Enabled ? Fluent.Text : Fluent.TextDisabled;

            using (SolidBrush brush = new SolidBrush(background))
                e.Graphics.FillRectangle(brush, e.Bounds);

            string text = e.Index >= 0 && e.Index < Items.Count ? GetItemText(Items[e.Index]) : Text;
            Rectangle textBounds = Rectangle.Inflate(e.Bounds, -Fluent.Scale(this, 10), 0);
            TextRenderer.DrawText(e.Graphics, text, Font, textBounds, foreground,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }
    }
}
