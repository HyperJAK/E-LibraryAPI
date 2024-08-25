namespace ELib_IDSFintech_Internship.Services.Tools
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Extensions.Configuration;

    public class AES256Encryption
    {
        private readonly byte[] _key;
        private readonly byte[] _initVector;

        public AES256Encryption(string key, string iv)
        {
            if (key.Length != 32 || iv.Length != 16)
            {
                throw new ArgumentException("Invalid key or IV length");
            }

            _key = Encoding.UTF8.GetBytes(key);
            _initVector = Encoding.UTF8.GetBytes(iv);
        }

        public AES256Encryption(IConfiguration configuration)
        {
            _key = Convert.FromHexString(configuration["Encryption:Key"]);
            _initVector = Convert.FromHexString(configuration["Encryption:InitVector"]);
        }

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _initVector;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _initVector;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

                using (MemoryStream ms = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
