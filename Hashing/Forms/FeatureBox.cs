using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hashing
{
    public partial class FeatureBox : Form
    {
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const int MF_BYCOMMAND = 0;
        const int MF_DISABLED = 2;
        const int SC_CLOSE = 0xF060;

        public static FeatureAction SelectedAction;

        public FeatureBox()
        {
            InitializeComponent();

            Options.ApplyTheme(this);
            CheckForIllegalCrossThreadCalls = false;

            label77.Text = Options.TranslationList["label77"].ToString();
            btnCalc.Text = Options.TranslationList["btnCalc"].ToString();
            btnAnalyze.Text = Options.TranslationList["btnAnalyze"].ToString();
            btnList.Text = Options.TranslationList["btnList"].ToString();

            var sm = GetSystemMenu(Handle, false);
            EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        private void FeatureBox_Load(object sender, EventArgs e)
        {

        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            SelectedAction = FeatureAction.Calculate;
            this.Close();
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            SelectedAction = FeatureAction.AnalyzeJSON;
            this.Close();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            SelectedAction = FeatureAction.ListJSON;
            this.Close();
        }
    }
}
