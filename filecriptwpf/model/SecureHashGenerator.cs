using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace filecriptwpf.model
{
    internal class SecureHashGenerator
    {
        public static string GetSha1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                return ByteArrayToHexString(hashBytes);
            }
        }
        public static string GetSha384Hash(string input)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha384.ComputeHash(inputBytes);

                return ByteArrayToHexString(hashBytes);
            }
        }
        public static byte[] ComputeSha512Hash(byte[] input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                
                byte[] hashBytes = sha512.ComputeHash(input);
                return hashBytes;
            }
        }

        private static string ByteArrayToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
