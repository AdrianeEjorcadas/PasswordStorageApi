using System.Security.Cryptography;
using System.Text;

namespace PasswordStorageApi.Helpers
{
    public class EncryptionHelper
    {

        private static readonly byte[] key = Encoding.UTF8.GetBytes("your-32-char-key-here");

        public static string Encrypt(string plainText, byte[] salt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = salt;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        byte[] result = new byte[salt.Length + encrypted.Length];
                        Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
                        Buffer.BlockCopy(encrypted, 0, result, salt.Length, encrypted.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] salt = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - salt.Length];

            Buffer.BlockCopy(fullCipher, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(fullCipher, salt.Length, cipher, 0, cipher.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = salt;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
