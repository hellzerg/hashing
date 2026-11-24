using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Hashing
{
    internal enum CertificatePurpose
    {
        TlsServer,
        TlsClient,
        CodeSigning,
        TlsServerAndClient,
        GeneralPurpose
    }

    internal sealed class CertificateGenerationOptions
    {
        internal string CommonName;
        internal string[] SubjectAlternativeNames;
        internal CertificatePurpose Purpose;
        internal int KeySize;
        internal int ValidityDays;
    }

    internal sealed class CertificateInspectionResult
    {
        internal string SourcePath;
        internal string Subject;
        internal string Issuer;
        internal string SerialNumber;
        internal string Thumbprint;
        internal string NotBefore;
        internal string NotAfter;
        internal string SignatureAlgorithm;
        internal string PublicKey;
        internal string KeyUsage;
        internal string EnhancedKeyUsage;
        internal string SubjectAlternativeNames;
        internal string BasicConstraints;
        internal string ChainStatus;
        internal bool HasPrivateKey;
        internal bool IsTimeValid;
        internal string Error;

        internal string GetDetails()
        {
            string notSpecified = ToolText.Get("certificateDetailsNotSpecified", "Not specified");
            if (!string.IsNullOrWhiteSpace(Error))
                return ToolText.Get("certificateDetailsFile", "File") + ": " + SourcePath + Environment.NewLine +
                    ToolText.Get("authenticodeDetailError", "Error") + ": " + Error;

            StringBuilder text = new StringBuilder();
            text.AppendLine(ToolText.Get("certificateDetailsFile", "File") + ": " + SourcePath);
            text.AppendLine(ToolText.Get("certificateDetailsSubject", "Subject") + ": " + Subject);
            text.AppendLine(ToolText.Get("certificateDetailsIssuer", "Issuer") + ": " + Issuer);
            text.AppendLine(ToolText.Get("certificateDetailsSerial", "Serial number") + ": " + SerialNumber);
            text.AppendLine(ToolText.Get("certificateDetailsThumbprint", "Thumbprint") + ": " + Thumbprint);
            text.AppendLine(ToolText.Get("certificateDetailsValidFrom", "Valid from") + ": " + NotBefore);
            text.AppendLine(ToolText.Get("certificateDetailsValidTo", "Valid to") + ": " + NotAfter);
            text.AppendLine(ToolText.Get("certificateDetailsTimeValidity", "Time validity") + ": " +
                ToolText.Get(IsTimeValid ? "certificateDetailsValid" : "certificateDetailsExpired", IsTimeValid ? "Valid" : "Expired or not yet valid"));
            text.AppendLine(ToolText.Get("certificateDetailsSignatureAlgorithm", "Signature algorithm") + ": " + SignatureAlgorithm);
            text.AppendLine(ToolText.Get("certificateDetailsPublicKey", "Public key") + ": " + PublicKey);
            text.AppendLine(ToolText.Get("certificateDetailsKeyUsage", "Key usage") + ": " + (KeyUsage ?? notSpecified));
            text.AppendLine(ToolText.Get("certificateDetailsEnhancedKeyUsage", "Enhanced key usage") + ": " + (EnhancedKeyUsage ?? notSpecified));
            text.AppendLine(ToolText.Get("certificateDetailsSan", "Subject alternative names") + ": " + (SubjectAlternativeNames ?? notSpecified));
            text.AppendLine(ToolText.Get("certificateDetailsBasicConstraints", "Basic constraints") + ": " + (BasicConstraints ?? notSpecified));
            text.AppendLine(ToolText.Get("certificateDetailsPrivateKey", "Private key included") + ": " +
                ToolText.Get(HasPrivateKey ? "certificateDetailsYes" : "certificateDetailsNo", HasPrivateKey ? "Yes" : "No"));
            text.AppendLine(ToolText.Get("certificateDetailsChain", "Chain") + ": " + ChainStatus);
            text.Append(ToolText.Get("certificateDetailsRevocation", "Revocation checking was not performed."));
            return text.ToString();
        }
    }

    internal static class CertificateService
    {
        private const string ServerAuthenticationOid = "1.3.6.1.5.5.7.3.1";
        private const string ClientAuthenticationOid = "1.3.6.1.5.5.7.3.2";
        private const string CodeSigningOid = "1.3.6.1.5.5.7.3.3";
        private static readonly Regex PemCertificate = new Regex(
            "-----BEGIN CERTIFICATE-----\\s*(?<data>[A-Za-z0-9+/\\r\\n=]+)\\s*-----END CERTIFICATE-----",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        internal static X509Certificate2 CreateSelfSigned(CertificateGenerationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.CommonName))
                throw new ArgumentException("A common name is required.", nameof(options));
            if (options.KeySize != 2048 && options.KeySize != 3072 && options.KeySize != 4096)
                throw new ArgumentOutOfRangeException(nameof(options.KeySize));
            if (options.ValidityDays < 1 || options.ValidityDays > 3650)
                throw new ArgumentOutOfRangeException(nameof(options.ValidityDays));

            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = options.KeySize;
                X500DistinguishedName subject = new X500DistinguishedName("CN=" + EscapeDistinguishedName(options.CommonName.Trim()));
                CertificateRequest request = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(GetKeyUsage(options.Purpose), true));
                request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
                AddEnhancedKeyUsage(request, options.Purpose);
                AddSubjectAlternativeNames(request, options.SubjectAlternativeNames, options.CommonName, options.Purpose);

                DateTimeOffset notBefore = DateTimeOffset.UtcNow.AddMinutes(-5);
                DateTimeOffset notAfter = DateTimeOffset.UtcNow.AddDays(options.ValidityDays);
                return request.CreateSelfSigned(notBefore, notAfter);
            }
        }

        internal static List<CertificateInspectionResult> InspectFiles(IEnumerable<string> filePaths, string password)
        {
            List<CertificateInspectionResult> results = new List<CertificateInspectionResult>();
            foreach (string path in filePaths ?? Enumerable.Empty<string>())
            {
                if (string.IsNullOrWhiteSpace(path)) continue;
                List<X509Certificate2> certificates = null;
                try
                {
                    certificates = LoadCertificates(path, password);
                    if (certificates.Count == 0)
                    {
                        results.Add(new CertificateInspectionResult { SourcePath = path, Error = "No certificates were found in the selected file." });
                        continue;
                    }

                    foreach (X509Certificate2 certificate in certificates)
                        results.Add(InspectCertificate(path, certificate));
                }
                catch (Exception ex)
                {
                    results.Add(new CertificateInspectionResult { SourcePath = path, Error = ex.Message });
                }
                finally
                {
                    if (certificates != null)
                    {
                        foreach (X509Certificate2 certificate in certificates)
                            certificate.Dispose();
                    }
                }
            }
            return results;
        }

        internal static void ExportPfxAndCer(X509Certificate2 certificate, string pfxPath, string password)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));
            if (!certificate.HasPrivateKey)
                throw new InvalidOperationException("The generated certificate does not contain a private key.");
            if (string.IsNullOrEmpty(password) || password.Length < 12)
                throw new ArgumentException("Use a password containing at least 12 characters.", nameof(password));

            string fullPfxPath = Path.GetFullPath(pfxPath);
            string cerPath = Path.ChangeExtension(fullPfxPath, ".cer");
            byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, password);
            byte[] cerBytes = certificate.Export(X509ContentType.Cert);

            string pfxTemporary = WriteTemporary(fullPfxPath, pfxBytes);
            string cerTemporary = null;
            try
            {
                cerTemporary = WriteTemporary(cerPath, cerBytes);
                ReplaceFile(cerTemporary, cerPath);
                cerTemporary = null;
                ReplaceFile(pfxTemporary, fullPfxPath);
                pfxTemporary = null;
            }
            finally
            {
                TryDelete(pfxTemporary);
                TryDelete(cerTemporary);
            }
        }

        private static List<X509Certificate2> LoadCertificates(string path, string password)
        {
            if (ContainsPemCertificate(path))
                return LoadPemCertificates(path);

            X509Certificate2Collection collection = new X509Certificate2Collection();
            try
            {
                collection.Import(path, password, X509KeyStorageFlags.EphemeralKeySet);
                return collection.Cast<X509Certificate2>().ToList();
            }
            catch
            {
                foreach (X509Certificate2 certificate in collection)
                    certificate.Dispose();
                throw;
            }
        }

        private static bool ContainsPemCertificate(string path)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.ASCII, true))
            {
                char[] header = new char[4096];
                int read = reader.Read(header, 0, header.Length);
                return read > 0 && new string(header, 0, read).IndexOf("-----BEGIN CERTIFICATE-----", StringComparison.Ordinal) >= 0;
            }
        }

        private static List<X509Certificate2> LoadPemCertificates(string path)
        {
            string text = File.ReadAllText(path, Encoding.UTF8);
            if (text.IndexOf("PRIVATE KEY-----", StringComparison.Ordinal) >= 0)
                throw new NotSupportedException(ToolText.Get(
                    "certificatePemPrivateKeyUnsupported",
                    "PEM private keys are not imported. Use a password-protected PFX/P12 file to inspect a certificate with its private key."));

            MatchCollection matches = PemCertificate.Matches(text);
            List<X509Certificate2> certificates = new List<X509Certificate2>(matches.Count);
            try
            {
                foreach (Match match in matches)
                {
                    byte[] raw = Convert.FromBase64String(Regex.Replace(match.Groups["data"].Value, @"\s+", string.Empty));
                    certificates.Add(new X509Certificate2(raw));
                }
                return certificates;
            }
            catch
            {
                foreach (X509Certificate2 certificate in certificates)
                    certificate.Dispose();
                throw;
            }
        }

        private static CertificateInspectionResult InspectCertificate(string sourcePath, X509Certificate2 certificate)
        {
            DateTime now = DateTime.Now;
            X509BasicConstraintsExtension basicConstraints = certificate.Extensions.OfType<X509BasicConstraintsExtension>().FirstOrDefault();
            X509KeyUsageExtension keyUsage = certificate.Extensions.OfType<X509KeyUsageExtension>().FirstOrDefault();
            X509EnhancedKeyUsageExtension enhancedUsage = certificate.Extensions.OfType<X509EnhancedKeyUsageExtension>().FirstOrDefault();
            X509Extension san = certificate.Extensions.Cast<X509Extension>().FirstOrDefault(extension => extension.Oid != null && extension.Oid.Value == "2.5.29.17");

            string publicKey = certificate.PublicKey.Oid.FriendlyName ?? certificate.PublicKey.Oid.Value;
            try
            {
                using (AsymmetricAlgorithm key = certificate.PublicKey.Key)
                {
                    if (key != null)
                        publicKey += " (" + key.KeySize.ToString(CultureInfo.InvariantCulture) + " bit)";
                }
            }
            catch { }

            string chainStatus;
            using (X509Chain chain = new X509Chain())
            {
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
                bool trusted;
                try
                {
                    trusted = chain.Build(certificate);
                    chainStatus = trusted
                        ? ToolText.Get("certificateDetailsChainBuilt", "Chain build succeeded (revocation not checked).")
                        : string.Join(", ", chain.ChainStatus.Select(status => status.Status.ToString()).Distinct());
                    if (!trusted && string.IsNullOrWhiteSpace(chainStatus))
                        chainStatus = ToolText.Get("certificateDetailsChainFailed", "The certificate chain could not be validated.");
                }
                catch (Exception ex)
                {
                    chainStatus = ToolText.Get("certificateDetailsChainFailed", "The certificate chain could not be validated.") + " " + ex.Message;
                }
            }

            return new CertificateInspectionResult
            {
                SourcePath = sourcePath,
                Subject = certificate.Subject,
                Issuer = certificate.Issuer,
                SerialNumber = certificate.SerialNumber,
                Thumbprint = certificate.Thumbprint,
                NotBefore = certificate.NotBefore.ToLocalTime().ToString("G", CultureInfo.CurrentCulture),
                NotAfter = certificate.NotAfter.ToLocalTime().ToString("G", CultureInfo.CurrentCulture),
                IsTimeValid = now >= certificate.NotBefore && now <= certificate.NotAfter,
                SignatureAlgorithm = certificate.SignatureAlgorithm.FriendlyName + " (" + certificate.SignatureAlgorithm.Value + ")",
                PublicKey = publicKey,
                KeyUsage = keyUsage == null ? null : keyUsage.Format(false),
                EnhancedKeyUsage = enhancedUsage == null
                    ? null
                    : string.Join(", ", enhancedUsage.EnhancedKeyUsages.Cast<Oid>().Select(oid => (oid.FriendlyName ?? oid.Value) + " [" + oid.Value + "]")),
                SubjectAlternativeNames = san == null ? null : san.Format(false),
                BasicConstraints = basicConstraints == null
                    ? null
                    : (basicConstraints.CertificateAuthority ? "CA" : "End entity") + (basicConstraints.HasPathLengthConstraint ? ", path length " + basicConstraints.PathLengthConstraint : string.Empty),
                ChainStatus = chainStatus,
                HasPrivateKey = certificate.HasPrivateKey
            };
        }

        private static X509KeyUsageFlags GetKeyUsage(CertificatePurpose purpose)
        {
            if (purpose == CertificatePurpose.CodeSigning)
                return X509KeyUsageFlags.DigitalSignature;
            return X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment;
        }

        private static void AddEnhancedKeyUsage(CertificateRequest request, CertificatePurpose purpose)
        {
            OidCollection usages = new OidCollection();
            switch (purpose)
            {
                case CertificatePurpose.TlsServer:
                    usages.Add(new Oid(ServerAuthenticationOid));
                    break;
                case CertificatePurpose.TlsClient:
                    usages.Add(new Oid(ClientAuthenticationOid));
                    break;
                case CertificatePurpose.CodeSigning:
                    usages.Add(new Oid(CodeSigningOid));
                    break;
                case CertificatePurpose.TlsServerAndClient:
                    usages.Add(new Oid(ServerAuthenticationOid));
                    usages.Add(new Oid(ClientAuthenticationOid));
                    break;
                case CertificatePurpose.GeneralPurpose:
                    return;
            }

            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(usages, true));
        }

        private static void AddSubjectAlternativeNames(
            CertificateRequest request,
            IEnumerable<string> names,
            string commonName,
            CertificatePurpose purpose)
        {
            List<string> entries = (names ?? Enumerable.Empty<string>())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if ((purpose == CertificatePurpose.TlsServer || purpose == CertificatePurpose.TlsServerAndClient) && entries.Count == 0)
                entries.Add(commonName.Trim());
            if (entries.Count == 0)
                return;

            SubjectAlternativeNameBuilder builder = new SubjectAlternativeNameBuilder();
            foreach (string entry in entries)
            {
                IPAddress address;
                if (IPAddress.TryParse(entry, out address))
                {
                    builder.AddIpAddress(address);
                    continue;
                }

                string dnsName = entry;
                bool wildcard = dnsName.StartsWith("*.", StringComparison.Ordinal);
                string normalized = wildcard ? dnsName.Substring(2) : dnsName;
                string ascii = new IdnMapping().GetAscii(normalized);
                builder.AddDnsName(wildcard ? "*." + ascii : ascii);
            }

            request.CertificateExtensions.Add(builder.Build(true));
        }

        private static string EscapeDistinguishedName(string value)
        {
            StringBuilder escaped = new StringBuilder(value.Length + 8);
            for (int i = 0; i < value.Length; i++)
            {
                char character = value[i];
                if (character == ',' || character == '+' || character == '"' || character == '\\' ||
                    character == '<' || character == '>' || character == ';' || character == '=')
                    escaped.Append('\\');
                if ((i == 0 && (character == '#' || character == ' ')) || (i == value.Length - 1 && character == ' '))
                    escaped.Append('\\');
                escaped.Append(character);
            }
            return escaped.ToString();
        }

        private static string WriteTemporary(string destination, byte[] bytes)
        {
            string directory = Path.GetDirectoryName(destination);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            string temporary = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
            File.WriteAllBytes(temporary, bytes);
            return temporary;
        }

        private static void ReplaceFile(string temporary, string destination)
        {
            if (File.Exists(destination))
                File.Replace(temporary, destination, null);
            else
                File.Move(temporary, destination);
        }

        private static void TryDelete(string path)
        {
            try { if (!string.IsNullOrWhiteSpace(path) && File.Exists(path)) File.Delete(path); }
            catch { }
        }
    }
}
