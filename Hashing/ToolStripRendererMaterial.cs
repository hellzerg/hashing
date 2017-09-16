using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashing
{
    public class ToolStripRendererMaterial : ToolStripProfessionalRenderer
    {
        public ToolStripRendererMaterial() : base(new ColorsMaterial())
        {

        }
    }

    public class ColorsMaterial : ProfessionalColorTable
    {
        public override Color ToolStripBorder
        {
            get
            {
                return Options.ForegroundColor;
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return Options.ForegroundColor;
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return Options.ForegroundAccentColor;
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Options.ForegroundAccentColor;
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Options.ForegroundAccentColor;
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return Options.ForegroundColor;
            }
        }
    }
}
