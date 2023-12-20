using System.Windows.Forms;
using System.Drawing;

namespace Hashing
{
    public static class TopMostMessageBox
    {
        public static DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (Form topmostForm = new Form { StartPosition = FormStartPosition.CenterScreen })
            {
                topmostForm.TopMost = true;
                return MessageBox.Show(topmostForm, message, title, buttons, icon);
            }
        }
    }
}
