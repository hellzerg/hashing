using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashing
{
    public static class TopMostMessageBox
    {
        static public DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon iconStyle)
        {
            Form topmostForm = new Form();
            
            topmostForm.Size = new System.Drawing.Size(1, 1);
            topmostForm.StartPosition = FormStartPosition.Manual;
            System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
            topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10, rect.Right + 10);
            topmostForm.Show();
            
            topmostForm.Focus();
            topmostForm.BringToFront();
            topmostForm.TopMost = true;
            
            DialogResult result = MessageBox.Show(topmostForm, message, title, buttons, iconStyle);
            topmostForm.Dispose();

            return result;
        }
    }
}
