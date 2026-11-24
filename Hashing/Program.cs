using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Hashing
{
    internal static class Program
    {
        /* VERSION PROPERTIES */
        /* DO NOT LEAVE THEM EMPTY */

        // Enter current version here
        internal readonly static float Major = 4;
        internal readonly static float Minor = 0;

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
        internal static Mutex MUTEX;
        static bool _notRunning;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool calculateFromShell = false;
            List<string> startupFiles = new List<string>();
            string[] arguments = args ?? new string[0];
            for (int i = 0; i < arguments.Length; i++)
            {
                string argument = arguments[i];
                if (i == 0 && string.Equals(argument, "--calculate", StringComparison.OrdinalIgnoreCase))
                    calculateFromShell = true;
                else if (!string.IsNullOrWhiteSpace(argument))
                    startupFiles.Add(argument);
            }

            if (calculateFromShell && startupFiles.Count > 1)
            {
                HashSet<string> uniqueFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                startupFiles = startupFiles.FindAll(uniqueFiles.Add);
            }

            string resource = "Hashing.Newtonsoft.Json.dll";
            string resource2 = "Hashing.Crc32.NET.dll";

            EmbeddedAssembly.Load(resource, "Newtonsoft.Json.dll");
            EmbeddedAssembly.Load(resource2, "Crc32.NET.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Options.LoadSettings();
            if (!calculateFromShell && ShellIntegration.IsRegistered())
            {
                try
                {
                    ShellIntegration.RegisterContextMenu(Options.TranslationList["shellContextMenuVerb"].ToString());
                }
                catch (Exception ex)
                {
                    Options.StartupWarnings.Add(Options.TranslationList["shellContextMenuFailed"].ToString().Replace("{ERROR}", ex.Message));
                }
            }

            if (Options.CurrentOptions.SingleInstance)
            {
                using (MUTEX = new Mutex(true, _mutexGuid, out _notRunning))
                {
                    if (_notRunning)
                    {
                        Application.Run(new MainForm(startupFiles.ToArray(), calculateFromShell, true));
                    }
                    else if (calculateFromShell && startupFiles.Count > 0)
                    {
                        if (ShellRequestPipe.TrySend(startupFiles.ToArray()))
                            return;

                        Application.Run(new MainForm(startupFiles.ToArray(), true, false));
                    }
                    else
                    {
                        FluentMessageBox.ShowTopMost("Hashing is already running in the background!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                Application.Run(new MainForm(startupFiles.ToArray(), calculateFromShell, false));
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}
