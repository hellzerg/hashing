using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hashing
{
    internal sealed class AuthenticodeInspectionResult
    {
        private const int NoSignature = unchecked((int)0x800B0100);

        internal string FilePath;
        internal string Publisher;
        internal string FileCompanyName;
        internal string ProductVersion;
        internal string FileVersion;
        internal string SignerSubject;
        internal string Issuer;
        internal string Thumbprint;
        internal string ValidFrom;
        internal string ValidTo;
        internal string SignatureAlgorithm;
        internal int TrustStatus;
        internal bool Trusted;
        internal bool SignerDetailsUnavailable;
        internal string Error;

        internal string GetStatusText()
        {
            if (!string.IsNullOrWhiteSpace(Error))
                return ToolText.Get("authenticodeStatusError", "Error");
            if (Trusted)
                return SignerDetailsUnavailable
                    ? ToolText.Get("authenticodeStatusSignerUnavailable", "Trusted (signer details unavailable)")
                    : ToolText.Get("authenticodeStatusTrusted", "Trusted (revocation not checked)");
            if (TrustStatus == NoSignature)
                return ToolText.Get("authenticodeStatusNoSignature", "No embedded signature");
            return ToolText.Get("authenticodeStatusUntrusted", "Invalid or untrusted signature");
        }

        internal string GetDetails()
        {
            StringBuilder details = new StringBuilder();
            details.AppendLine(ToolText.Get("authenticodeDetailFile", "File") + ": " + FilePath);
            details.AppendLine(ToolText.Get("authenticodeDetailStatus", "Status") + ": " + GetStatusText());
            if (!string.IsNullOrWhiteSpace(Error))
            {
                details.AppendLine(ToolText.Get("authenticodeDetailError", "Error") + ": " + Error);
                return details.ToString().TrimEnd();
            }

            details.AppendLine(ToolText.Get("authenticodeDetailSigner", "Signer") + ": " + (Publisher ?? ToolText.Get("notAvailable", "Not available")));
            details.AppendLine(ToolText.Get("authenticodeDetailFileCompany", "File company metadata") + ": " + (FileCompanyName ?? ToolText.Get("notAvailable", "Not available")));
            details.AppendLine(ToolText.Get("authenticodeDetailProductVersion", "Product version") + ": " + (ProductVersion ?? ToolText.Get("notAvailable", "Not available")));
            details.AppendLine(ToolText.Get("authenticodeDetailFileVersion", "File version") + ": " + (FileVersion ?? ToolText.Get("notAvailable", "Not available")));
            if (!string.IsNullOrWhiteSpace(SignerSubject)) details.AppendLine(ToolText.Get("authenticodeDetailSubject", "Signer subject") + ": " + SignerSubject);
            if (!string.IsNullOrWhiteSpace(Issuer)) details.AppendLine(ToolText.Get("authenticodeDetailIssuer", "Issuer") + ": " + Issuer);
            if (!string.IsNullOrWhiteSpace(Thumbprint)) details.AppendLine(ToolText.Get("authenticodeDetailThumbprint", "Thumbprint") + ": " + Thumbprint);
            if (!string.IsNullOrWhiteSpace(ValidFrom)) details.AppendLine(ToolText.Get("authenticodeDetailValidFrom", "Valid from") + ": " + ValidFrom);
            if (!string.IsNullOrWhiteSpace(ValidTo)) details.AppendLine(ToolText.Get("authenticodeDetailValidTo", "Valid to") + ": " + ValidTo);
            if (!string.IsNullOrWhiteSpace(SignatureAlgorithm)) details.AppendLine(ToolText.Get("authenticodeDetailAlgorithm", "Signature algorithm") + ": " + SignatureAlgorithm);
            if (TrustStatus != 0)
                details.AppendLine(ToolText.Get("authenticodeDetailTrustStatus", "WinVerifyTrust status") + ": 0x" + unchecked((uint)TrustStatus).ToString("X8"));
            details.Append(ToolText.Get("authenticodeDetailRevocation", "Revocation was not checked (cache-only verification)."));
            return details.ToString().TrimEnd();
        }
    }

    internal static class AuthenticodeService
    {
        private static readonly Guid GenericVerifyV2 = new Guid("00AAC56B-CD44-11D0-8CC2-00C04FC295EE");
        private const uint UiChoiceNone = 2;
        private const uint UnionChoiceFile = 1;
        private const uint StateActionVerify = 1;
        private const uint StateActionClose = 2;
        private const uint RevocationCheckNone = 0;
        private const uint CacheOnlyUrlRetrieval = 0x00001000;
        private const uint RevocationCheckNoneFlag = 0x00000010;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WinTrustFileInfo
        {
            internal uint StructSize;
            internal IntPtr FilePath;
            internal IntPtr FileHandle;
            internal IntPtr KnownSubject;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WinTrustData
        {
            internal uint StructSize;
            internal IntPtr PolicyCallbackData;
            internal IntPtr SipClientData;
            internal uint UiChoice;
            internal uint RevocationChecks;
            internal uint UnionChoice;
            internal IntPtr FileInfo;
            internal uint StateAction;
            internal IntPtr StateData;
            internal IntPtr UrlReference;
            internal uint ProviderFlags;
            internal uint UiContext;
            internal IntPtr SignatureSettings;
        }

        [DllImport("wintrust.dll", ExactSpelling = true, PreserveSig = true)]
        private static extern int WinVerifyTrust(IntPtr windowHandle, ref Guid actionId, IntPtr trustData);

        internal static AuthenticodeInspectionResult Inspect(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A file path is required.", nameof(filePath));

            string fullPath = Path.GetFullPath(filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("The selected file was not found.", fullPath);

            AuthenticodeInspectionResult result = new AuthenticodeInspectionResult { FilePath = fullPath };
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(fullPath);
            result.ProductVersion = version.ProductVersion;
            result.FileVersion = version.FileVersion;
            result.FileCompanyName = version.CompanyName;

            result.TrustStatus = VerifyTrust(fullPath);
            result.Trusted = result.TrustStatus == 0;

            try
            {
                using (X509Certificate signer = X509Certificate.CreateFromSignedFile(fullPath))
                using (X509Certificate2 certificate = new X509Certificate2(signer))
                {
                    result.SignerSubject = certificate.Subject;
                    result.Publisher = certificate.GetNameInfo(X509NameType.SimpleName, false);
                    result.Issuer = certificate.Issuer;
                    result.Thumbprint = certificate.Thumbprint;
                    result.ValidFrom = certificate.NotBefore.ToLocalTime().ToString("G");
                    result.ValidTo = certificate.NotAfter.ToLocalTime().ToString("G");
                    result.SignatureAlgorithm = certificate.SignatureAlgorithm == null
                        ? null
                        : certificate.SignatureAlgorithm.FriendlyName + " (" + certificate.SignatureAlgorithm.Value + ")";
                }
            }
            catch (CryptographicException)
            {
                if (result.Trusted)
                    result.SignerDetailsUnavailable = true;
            }

            return result;
        }

        private static int VerifyTrust(string filePath)
        {
            IntPtr pathPointer = IntPtr.Zero;
            IntPtr fileInfoPointer = IntPtr.Zero;
            IntPtr trustDataPointer = IntPtr.Zero;

            Guid actionId = GenericVerifyV2;
            try
            {
                pathPointer = Marshal.StringToCoTaskMemUni(filePath);
                WinTrustFileInfo fileInfo = new WinTrustFileInfo
                {
                    StructSize = (uint)Marshal.SizeOf(typeof(WinTrustFileInfo)),
                    FilePath = pathPointer,
                    FileHandle = IntPtr.Zero,
                    KnownSubject = IntPtr.Zero
                };

                fileInfoPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinTrustFileInfo)));
                Marshal.StructureToPtr(fileInfo, fileInfoPointer, false);

                WinTrustData data = new WinTrustData
                {
                    StructSize = (uint)Marshal.SizeOf(typeof(WinTrustData)),
                    PolicyCallbackData = IntPtr.Zero,
                    SipClientData = IntPtr.Zero,
                    UiChoice = UiChoiceNone,
                    RevocationChecks = RevocationCheckNone,
                    UnionChoice = UnionChoiceFile,
                    FileInfo = fileInfoPointer,
                    StateAction = StateActionVerify,
                    StateData = IntPtr.Zero,
                    UrlReference = IntPtr.Zero,
                    ProviderFlags = CacheOnlyUrlRetrieval | RevocationCheckNoneFlag,
                    UiContext = 0,
                    SignatureSettings = IntPtr.Zero
                };

                trustDataPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinTrustData)));
                Marshal.StructureToPtr(data, trustDataPointer, false);
                int status = WinVerifyTrust(IntPtr.Zero, ref actionId, trustDataPointer);

                WinTrustData verifiedData = (WinTrustData)Marshal.PtrToStructure(trustDataPointer, typeof(WinTrustData));
                verifiedData.StateAction = StateActionClose;
                Marshal.StructureToPtr(verifiedData, trustDataPointer, false);
                WinVerifyTrust(IntPtr.Zero, ref actionId, trustDataPointer);
                return status;
            }
            finally
            {
                if (trustDataPointer != IntPtr.Zero) Marshal.FreeHGlobal(trustDataPointer);
                if (fileInfoPointer != IntPtr.Zero) Marshal.FreeHGlobal(fileInfoPointer);
                if (pathPointer != IntPtr.Zero) Marshal.FreeCoTaskMem(pathPointer);
            }
        }
    }
}
