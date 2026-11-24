using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Hashing
{
    internal static class ShellIntegration
    {
        private const string ContextMenuPath = @"Software\Classes\*\shell\Hashing.Calculate";
        private const string RegistrationOwner = "hellzerg.Hashing";
        private const uint AssociationChanged = 0x08000000;
        private const uint ChangeNotifyIdList = 0x0000;

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint eventId, uint flags, IntPtr item1, IntPtr item2);

        internal static bool IsRegistered()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ContextMenuPath, false))
                {
                    if (key == null || !string.Equals(
                        key.GetValue("HashingOwner") as string,
                        RegistrationOwner,
                        StringComparison.Ordinal))
                        return false;

                    using (RegistryKey command = key.OpenSubKey("command", false))
                    {
                        string commandLine = command == null ? null : command.GetValue(string.Empty) as string;
                        return !string.IsNullOrWhiteSpace(commandLine) &&
                            commandLine.IndexOf("--calculate", StringComparison.OrdinalIgnoreCase) >= 0 &&
                            commandLine.IndexOf("%*", StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        internal static void RegisterContextMenu(string menuText)
        {
            using (RegistryKey existing = Registry.CurrentUser.OpenSubKey(ContextMenuPath, false))
            {
                if (existing != null && !string.Equals(
                    existing.GetValue("HashingOwner") as string,
                    RegistrationOwner,
                    StringComparison.Ordinal))
                    throw new InvalidOperationException("A different application already owns the Hashing context-menu registry key.");
            }

            string executablePath = Application.ExecutablePath;
            string commandLine = "\"" + executablePath + "\" --calculate \"%1\" %*";

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ContextMenuPath))
            {
                if (key == null)
                    throw new InvalidOperationException("Windows could not create the context-menu registry key.");

                string displayText = string.IsNullOrWhiteSpace(menuText) ? "Calculate with Hashing" : menuText;
                key.SetValue(string.Empty, displayText, RegistryValueKind.String);
                key.SetValue("HashingOwner", RegistrationOwner, RegistryValueKind.String);
                key.SetValue("MUIVerb", displayText, RegistryValueKind.String);
                key.SetValue("Icon", "\"" + executablePath + "\",0", RegistryValueKind.String);
                key.SetValue("MultiSelectModel", "Player", RegistryValueKind.String);

                using (RegistryKey command = key.CreateSubKey("command"))
                {
                    if (command == null)
                        throw new InvalidOperationException("Windows could not create the context-menu command.");

                    command.SetValue(string.Empty, commandLine, RegistryValueKind.String);
                }
            }

            NotifyShell();
        }

        internal static void UnregisterContextMenu()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ContextMenuPath, false))
            {
                if (key == null)
                    return;

                if (!string.Equals(key.GetValue("HashingOwner") as string, RegistrationOwner, StringComparison.Ordinal))
                    throw new InvalidOperationException("The existing context-menu entry is not owned by Hashing and was not removed.");
            }

            Registry.CurrentUser.DeleteSubKeyTree(ContextMenuPath, false);
            NotifyShell();
        }

        private static void NotifyShell()
        {
            try
            {
                SHChangeNotify(AssociationChanged, ChangeNotifyIdList, IntPtr.Zero, IntPtr.Zero);
            }
            catch
            {
            }
        }
    }

    internal static class ShellRequestPipe
    {
        private static string _pipeName;

        internal static string PipeName
        {
            get
            {
                if (_pipeName != null)
                    return _pipeName;

                string sid = Environment.UserName;
                try
                {
                    using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                    {
                        if (identity.User != null)
                            sid = identity.User.Value;
                    }
                }
                catch
                {
                }

                StringBuilder safeSid = new StringBuilder(sid.Length);
                foreach (char character in sid)
                    safeSid.Append(char.IsLetterOrDigit(character) ? character : '_');

                using (Process process = Process.GetCurrentProcess())
                    _pipeName = "HashingShellRequests_" + process.SessionId.ToString() + "_" + safeSid;

                return _pipeName;
            }
        }

        internal static bool TrySend(string[] files)
        {
            if (files == null || files.Length == 0)
                return false;

            byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(files));
            for (int attempt = 0; attempt < 20; attempt++)
            {
                try
                {
                    using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.None))
                    {
                        pipe.Connect(200);
                        pipe.Write(payload, 0, payload.Length);
                        pipe.Flush();
                    }

                    return true;
                }
                catch (TimeoutException)
                {
                    Thread.Sleep(100);
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
                catch (UnauthorizedAccessException)
                {
                    return false;
                }
            }

            return false;
        }
    }

    internal sealed class ShellRequestServer : IDisposable
    {
        private readonly Action<string[]> _handler;
        private readonly Thread _listenerThread;
        private readonly object _sync = new object();
        private NamedPipeServerStream _activePipe;
        private int _disposed;

        internal ShellRequestServer(Action<string[]> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _handler = handler;
            _listenerThread = new Thread(Listen)
            {
                IsBackground = true,
                Name = "Hashing Explorer request listener"
            };
            _listenerThread.Start();
        }

        private void Listen()
        {
            while (Volatile.Read(ref _disposed) == 0)
            {
                NamedPipeServerStream pipe = null;
                try
                {
                    pipe = new NamedPipeServerStream(
                        ShellRequestPipe.PipeName,
                        PipeDirection.In,
                        1,
                        PipeTransmissionMode.Byte,
                        PipeOptions.None,
                        4096,
                        4096,
                        CreatePipeSecurity());

                    lock (_sync)
                    {
                        if (Volatile.Read(ref _disposed) != 0)
                            return;
                        _activePipe = pipe;
                    }

                    pipe.WaitForConnection();
                    using (MemoryStream buffer = new MemoryStream())
                    {
                        pipe.CopyTo(buffer);
                        string payload = Encoding.UTF8.GetString(buffer.ToArray());
                        string[] files = JsonConvert.DeserializeObject<string[]>(payload);
                        if (files != null && files.Length > 0 && Volatile.Read(ref _disposed) == 0)
                            _handler(files);
                    }
                }
                catch (IOException)
                {
                    if (Volatile.Read(ref _disposed) != 0)
                        return;
                }
                catch (ObjectDisposedException)
                {
                    if (Volatile.Read(ref _disposed) != 0)
                        return;
                }
                catch
                {
                }
                finally
                {
                    lock (_sync)
                    {
                        if (ReferenceEquals(_activePipe, pipe))
                            _activePipe = null;
                    }

                    if (pipe != null)
                        pipe.Dispose();
                }
            }
        }

        private static PipeSecurity CreatePipeSecurity()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                SecurityIdentifier userSid = identity.User;
                if (userSid == null)
                    throw new InvalidOperationException("Unable to determine the current Windows user SID for the Explorer request pipe.");

                PipeSecurity security = new PipeSecurity();
                security.SetAccessRuleProtection(true, false);
                security.AddAccessRule(new PipeAccessRule(userSid, PipeAccessRights.FullControl, AccessControlType.Allow));
                return security;
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            lock (_sync)
            {
                if (_activePipe != null)
                {
                    try { _activePipe.Dispose(); }
                    catch { }
                    _activePipe = null;
                }
            }

            // The listener is a background thread. Closing its active pipe wakes it
            // without blocking the UI thread while the application is shutting down.
        }
    }
}
