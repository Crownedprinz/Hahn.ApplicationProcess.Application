using System;
using System.IO;
using System.Security.Cryptography;

namespace HAF.Security
{
    public class RsaKeyProvider : IRsaKeyProvider
    {
        private readonly string _rsaKeyPath;

        public RsaKeyProvider()
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RsaKeys");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            _rsaKeyPath = Path.Combine(folder, "RsaUserKey.txt");
        }

        public string GetPrivateAndPublicKey()
        {
            var result = ReadPrivateAndPublicKey();
            if (!string.IsNullOrEmpty(result))
                return result;

            var key = CreatePrivateAndPublicKey();
            InsertPrivateAndPublicKey(key);
            return key;
        }

        public string GetPublicKey()
        {
            var provider = new RSACryptoServiceProvider();
            provider.FromXmlString(ReadPrivateAndPublicKey());
            return provider.ToXmlString(false);
        }

        private static string CreatePrivateAndPublicKey()
        {
            var provider = new RSACryptoServiceProvider(2048);
            return provider.ToXmlString(true);
        }

        private void InsertPrivateAndPublicKey(string key)
        {
            using (var fileStream = new StreamWriter(_rsaKeyPath))
            {
                fileStream.WriteLine(key);
            }
        }

        private string ReadPrivateAndPublicKey()
        {
            if (!File.Exists(_rsaKeyPath))
                return null;

            using (var fileStream = new StreamReader(_rsaKeyPath))
            {
                return fileStream.ReadToEnd();
            }
        }
    }
}