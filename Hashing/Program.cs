using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Hashing
{
    static class Program
    {
        internal static readonly float Major = 3;
        internal static readonly float Minor = 7;

        internal static string GetCurrentVersionToString() => $"{Major}.{Minor}";
        internal static float GetCurrentVersion() => float.Parse(GetCurrentVersionToString());

        private const string MutexGuid = @"{DEADMOON-0EFC7B9A-D7FC-437F-B4B3-0118C643FE19-HASHING}";
        internal static Mutex MUTEX;
        private static bool notRunning;

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoadEmbeddedAssemblies();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) => EmbeddedAssembly.Get(e.Name);
            Options.LoadSettings();

            if (Options.CurrentOptions.SingleInstance)
            {
                using (MUTEX = new Mutex(true, MutexGuid, out notRunning))
                {
                    if (notRunning)
                    {
                        Application.Run(new MainForm(args));
                    }
                    else
                    {
                        MessageBox.Show("Hashing is already running in the background!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                Application.Run(new MainForm(args));
            }
        }

        private static void LoadEmbeddedAssemblies()
        {
            EmbeddedAssembly.Load("Hashing.Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
            EmbeddedAssembly.Load("Hashing.Crc32.NET.dll", "Crc32.NET.dll");
        }
    }
}
