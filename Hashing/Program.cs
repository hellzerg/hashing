using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Hashing
{
    static class Program
    {
        /* VERSION PROPERTIES */
        /* DO NOT LEAVE THEM EMPTY */

        // Enter current version here
        internal readonly static float Major = 2;
        internal readonly static float Minor = 2;

        /* END OF VERSION PROPERTIES */

        internal static string GetCurrentVersionToString()
        {
            return Major.ToString() + "." + Minor.ToString();
        }

        internal static float GetCurrentVersion()
        {
            return float.Parse(GetCurrentVersionToString());
        }

        const string _mutexGuid = @"{DEADMOON-0EFC7B9A-D7FC-437F-B4B3-0118C643FE19-HASHING}";
        static bool _notRunning;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string resource = "Hashing.Newtonsoft.Json.dll";
            string resource2 = "Hashing.Crc32.NET.dll";

            EmbeddedAssembly.Load(resource, "Newtonsoft.Json.dll");
            EmbeddedAssembly.Load(resource2, "Crc32.NET.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Options.LoadSettings();

            if (Options.CurrentOptions.SingleInstance)
            {
                using (Mutex m = new Mutex(true, _mutexGuid, out _notRunning))
                {
                    if (_notRunning)
                    {
                        Application.Run(new MainForm(args));
                    }
                    else
                    {
                        MessageBox.Show("Hashing is already running in the background!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                }
            }
            else
            {
                Application.Run(new MainForm(args));
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}
