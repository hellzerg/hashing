﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;
using Force.Crc32;

namespace Hashing
{
    internal static class Utilities
    {
        internal static SumResult CalculateSums(string file)
        {
            SumResult result = new SumResult();
            
            try
            {
                if (File.Exists(file))
                {
                    result.File = file;

                    if (Options.CurrentOptions.HashOptions.MD5)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var md5 = new MD5CryptoServiceProvider())
                            {
                                result.MD5 = BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", string.Empty);
                            }
                        }
                    }
                    
                    if (Options.CurrentOptions.HashOptions.SHA1)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var sha1 = new SHA1CryptoServiceProvider())
                            {
                                result.SHA1 = BitConverter.ToString(sha1.ComputeHash(fs)).Replace("-", string.Empty);
                            }
                        }
                    }

                    if (Options.CurrentOptions.HashOptions.SHA256)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var sha256 = new SHA256CryptoServiceProvider())
                            {
                                result.SHA256 = BitConverter.ToString(sha256.ComputeHash(fs)).Replace("-", string.Empty);
                            }
                        }
                    }

                    if (Options.CurrentOptions.HashOptions.SHA384)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var sha384 = new SHA384CryptoServiceProvider())
                            {
                                result.SHA384 = BitConverter.ToString(sha384.ComputeHash(fs)).Replace("-", string.Empty);
                            }
                        }
                    }

                    if (Options.CurrentOptions.HashOptions.SHA512)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var sha512 = new SHA512CryptoServiceProvider())
                            {
                                result.SHA512 = BitConverter.ToString(sha512.ComputeHash(fs)).Replace("-", string.Empty);
                            }
                        }
                    }

                    if (Options.CurrentOptions.HashOptions.CRC32)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var crc32 = new Crc32Algorithm())
                            {
                                result.CRC32 = BitConverter.ToString(crc32.ComputeHash(fs)).Replace("-", string.Empty);
                                
                                if (Options.CurrentOptions.CRC32Decimal)
                                {
                                    result.CRC32 = Convert.ToInt64(result.CRC32, 16).ToString();
                                }
                            }
                        }
                    }

                    if (Options.CurrentOptions.HashOptions.RIPEMD160)
                    {
                        using (FileStream fs = File.OpenRead(file))
                        {
                            using (var ripemd160 = RIPEMD160.Create())
                            {
                                result.RIPEMD160 = BitConverter.ToString(ripemd160.ComputeHash(fs)).Replace ("-", string.Empty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                MessageBox.Show(ex.Message, "Hash Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (Options.CurrentOptions.LowerCasing)
            {
                result.ConvertToLowerCasing();
            }
            else
            {
                result.ConvertToUpperCasing(); 
            }

            return result;
        }

        internal static void CopyToClipboard(string text, bool all = false)
        {
            try
            {
                if (!all)
                {
                    text = text.Replace("MD5:", string.Empty);
                    text = text.Replace("SHA1:", string.Empty);
                    text = text.Replace("SHA256:", string.Empty);
                    text = text.Replace("SHA384:", string.Empty);
                    text = text.Replace("SHA512:", string.Empty);
                    text = text.Replace("CRC32:", string.Empty);
                    text = text.Replace("RIPEMD160:", string.Empty);
                }

                Clipboard.SetText(text.Trim());
            }
            catch { }
        }

        internal static void EnableHighPriority()
        {
            Process p = Process.GetCurrentProcess();
            p.PriorityClass = ProcessPriorityClass.High;
        }

        internal static void DisableHighPriority()
        {
            Process p = Process.GetCurrentProcess();
            p.PriorityClass = ProcessPriorityClass.Normal;
        }

        internal static void SearchVirusTotal(string hash)
        {
            try
            {
                Process.Start(string.Format("https://www.virustotal.com/#/file/{0}/detection", hash.ToLowerInvariant()));
            }
            catch { }
        }
    }
}
