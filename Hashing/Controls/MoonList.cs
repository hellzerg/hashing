using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    public sealed class MoonList : ListBox
    {
        public MoonList()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;
            ItemHeight = 28;
            IntegralHeight = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color background = selected ? Fluent.SelectionColor : Fluent.Surface;
            Color foreground = Enabled ? Fluent.Text : Fluent.TextDisabled;

            using (SolidBrush brush = new SolidBrush(background))
                e.Graphics.FillRectangle(brush, e.Bounds);

            Rectangle textBounds = new Rectangle(e.Bounds.Left + 10, e.Bounds.Top, e.Bounds.Width - 18, e.Bounds.Height);
            Fluent.DrawText(e.Graphics, Items[e.Index].ToString(), Font, textBounds, foreground,
                TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();
        }
    }
}
