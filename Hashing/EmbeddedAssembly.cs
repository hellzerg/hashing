using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Hashing
{
    public class EmbeddedAssembly
    {
        private static readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public static void Load(string embeddedResource, string fileName)
        {
            var assemblyData = GetEmbeddedResourceBytes(embeddedResource);
            var assembly = TryLoadAssemblyFromBytes(assemblyData);

            if (assembly == null)
            {
                var tempFilePath = GetOrCreateTemporaryFile(assemblyData, fileName);
                assembly = Assembly.LoadFile(tempFilePath);
            }

            _assemblies[assembly.FullName] = assembly;
        }

        public static Assembly Get(string assemblyFullName)
        {
            _assemblies.TryGetValue(assemblyFullName, out var assembly);
            return assembly;
        }

        private static byte[] GetEmbeddedResourceBytes(string embeddedResource)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream(embeddedResource))
            {
                if (stream == null)
                    throw new Exception($"{embeddedResource} is not found in Embedded Resources.");

                var resourceBytes = new byte[stream.Length];
                stream.Read(resourceBytes, 0, resourceBytes.Length);
                return resourceBytes;
            }
        }

        private static Assembly TryLoadAssemblyFromBytes(byte[] assemblyData)
        {
            try
            {
                return Assembly.Load(assemblyData);
            }
            catch
            {
                return null;
            }
        }

        private static string GetOrCreateTemporaryFile(byte[] assemblyData, string fileName)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            if (!IsFileContentEqual(tempFilePath, assemblyData))
            {
                File.WriteAllBytes(tempFilePath, assemblyData);
            }
            return tempFilePath;
        }

        private static bool IsFileContentEqual(string filePath, byte[] data)
        {
            if (!File.Exists(filePath)) return false;

            var fileData = File.ReadAllBytes(filePath);
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var fileHash = BitConverter.ToString(sha1.ComputeHash(fileData)).Replace("-", string.Empty);
                var dataHash = BitConverter.ToString(sha1.ComputeHash(data)).Replace("-", string.Empty);

                return fileHash == dataHash;
            }
        }
    }
}
