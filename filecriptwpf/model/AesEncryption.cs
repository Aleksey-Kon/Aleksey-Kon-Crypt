using System;
using System.IO;
using System.Security.Cryptography;

namespace filecriptwpf.model { 
public class AesEncryption
{
    // Метод для шифрования данных
    public static byte[] Encrypt(byte[] data, string password)
    {
        using (Aes aesAlg = Aes.Create())
        {
            // Генерация ключа и вектора инициализации (IV) на основе пароля
            var key = new Rfc2898DeriveBytes(password, aesAlg.IV, 1000).GetBytes(32);
            var iv = aesAlg.IV;

            using (var encryptor = aesAlg.CreateEncryptor(key, iv))
            using (var msEncrypt = new MemoryStream())
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
        using (Aes aesAlg = Aes.Create())
        {
            // Извлекаем IV из начала зашифрованных данных
            var iv = new byte[aesAlg.BlockSize / 8];
            Array.Copy(encryptedData, 0, iv, 0, iv.Length);

            // Генерация ключа на основе пароля и извлеченного IV
            var key = new Rfc2898DeriveBytes(password, iv, 1000).GetBytes(32);

            using (var decryptor = aesAlg.CreateDecryptor(key, iv))
            using (var msDecrypt = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var msOutput = new MemoryStream())
            {
                csDecrypt.CopyTo(msOutput);
                return msOutput.ToArray();
            }
        }
    }
}
}