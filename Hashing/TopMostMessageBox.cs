using System.Windows.Forms;

namespace Hashing
{
    public static class TopMostMessageBox
    {
        public static DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon iconStyle)
        {
            return FluentMessageBox.ShowTopMost(message, title, buttons, iconStyle);
        }
    }
}
