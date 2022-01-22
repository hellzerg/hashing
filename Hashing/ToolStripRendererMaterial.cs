using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    internal class ToolStripRendererMaterial : ToolStripProfessionalRenderer
    {
        internal ToolStripRendererMaterial() : base(new ColorsMaterial())
        {

        }

        //protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        //{
        //    var tsMenuItem = e.Item as ToolStripMenuItem;
        //    if (tsMenuItem != null)
        //        e.ArrowColor = Options.ForegroundColor;
        //    base.OnRenderArrow(e);
        //}
    }

    internal class ColorsMaterial : ProfessionalColorTable
    {
        public override Color SeparatorLight
        {
            get { return Color.FromArgb(40, 40, 40); }
        }

        public override Color SeparatorDark
        {
            get { return Color.FromArgb(40, 40, 40); }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Options.BackgroundColor;
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return Options.BackgroundColor;
            }
        }
        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return Options.BackgroundColor;
            }
        }
        public override Color ImageMarginGradientEnd
        {
            get
            {
                return Options.BackgroundColor;
            }
        }

        public override Color ToolStripBorder
        {
            get
            {
                return Options.BackgroundColor;
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return Color.FromArgb(40, 40, 40);
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return Color.FromArgb(40, 40, 40);
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Color.FromArgb(40, 40, 40);
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Color.FromArgb(40, 40, 40);
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return Color.FromArgb(40, 40, 40);
            }
        }
    }
}
