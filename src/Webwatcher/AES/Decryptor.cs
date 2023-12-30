using System;
using System.Security.Cryptography;
using System.Text;

namespace Webwatcher.AES
{
    internal static class Decryptor
    {
        /// <summary>
        /// Decrypt a key
        /// </summary>
        /// <param name="ciphertext">The key to decrypt</param>
        /// <returns>The decrypted key</returns>
        internal static string ToString(byte[] key)
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Key = new byte[] { 0x3C, 0x4E, 0x80, 0x0F, 0xFF, 0x79, 0xD2, 0xC1, 0xCF, 0x52, 0x39, 0xCC, 0xEB, 0x0E, 0x9E, 0x5B, 0xF2, 0x6A, 0x9F, 0x20, 0xD9, 0x75, 0x44, 0xAE, 0x35, 0xD3, 0xBF, 0x59, 0x18, 0x0C, 0x1D, 0xA7 };
            aes.IV = new byte[] { 0x14, 0x7C, 0x40, 0x76, 0x7B, 0x57, 0x3A, 0x22, 0x17, 0x9B, 0xFC, 0x86, 0x5D, 0xF4, 0x90, 0x36 };

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] decryptedBytes = decryptor.TransformFinalBlock(key, 0, key.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
