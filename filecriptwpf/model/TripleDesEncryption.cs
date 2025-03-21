using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace filecriptwpf
{
    public class TripleDesEncryption
    {
        // Метод для шифрования данных
        public static byte[] Encrypt(byte[] data, string password)
        {
            using (TripleDES tripleDes = TripleDES.Create())
            {
                // Генерация ключа и вектора инициализации (IV) на основе пароля
                var key = new Rfc2898DeriveBytes(password, tripleDes.IV, 1000).GetBytes(24); // 24 байта для 3DES
                var iv = tripleDes.IV;

                using (var encryptor = tripleDes.CreateEncryptor(key, iv))
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    // Записываем IV в начало потока, чтобы его можно было использовать при дешифровании
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        // Метод для дешифрования данных
        public static byte[] Decrypt(byte[] encryptedData, string password)
        {
            using (TripleDES tripleDes = TripleDES.Create())
            {
                // Извлекаем IV из начала зашифрованных данных
                var iv = new byte[tripleDes.BlockSize / 8];
                Array.Copy(encryptedData, 0, iv, 0, iv.Length);

                // Генерация ключа на основе пароля и извлеченного IV
                var key = new Rfc2898DeriveBytes(password, iv, 1000).GetBytes(24); // 24 байта для 3DES

                using (var decryptor = tripleDes.CreateDecryptor(key, iv))
                using (var msDecrypt = new System.IO.MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var msOutput = new System.IO.MemoryStream())
                {
                    csDecrypt.CopyTo(msOutput);
                    return msOutput.ToArray();
                }
            }
        }
    }
}
