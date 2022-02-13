using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    public class MoonTree : TreeView
    {
        public MoonTree()
        {
            this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;
            this.BorderStyle = BorderStyle.None;
            
            this.NodeMouseClick += MoonTree_NodeMouseClick;
        }

        private void MoonTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                this.SelectedNode = e.Node;
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            Rectangle r = new Rectangle();
            r.X = e.Bounds.X;
            r.Y = e.Bounds.Y;
            r.Height = e.Bounds.Height;
            r.Width = e.Bounds.Width; //100000

            if (e.Node.IsSelected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, 50, 50)), r); //e.Bounds
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, 20, 20)), r); //e.Bounds
            }

            if (e.Node.Nodes.Count > 0)
            {
                //r.X = 0;
                e.Graphics.DrawString(e.Node.Text, this.Font, new SolidBrush(Options.ForegroundColor), r);
               // TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Node.Bounds, Options.ForegroundColor);
            }
            else
            {
                r.X += 20;
                e.Graphics.DrawString(e.Node.Text, this.Font, new SolidBrush(Color.White), r);
               // TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Node.Bounds, Color.White);
            }

            if (this.ImageList != null && this.ImageList.Images.Count > 0 && e.Node.SelectedImageIndex > -1)
            {
                e.Graphics.DrawImage(this.ImageList.Images[e.Node.SelectedImageIndex], e.Bounds.Left + 15 * e.Node.Level + 5, e.Bounds.Top);
            }
        }
    }
}
