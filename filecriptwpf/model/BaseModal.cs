using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filecriptwpf.model
{
    internal class BaseModal
    {
        public static void Encript(string value, int shift, string key, int keyshift)
        {

            byte[] valueB = File.ReadAllBytes(value);
            valueB = AesEncryption.Encrypt(valueB, key);
            valueB = TripleDesEncryption.Encrypt(valueB, key);
            byte[] keyB = ConvetByte(key);

            keyB = CaesarCipher(keyB.ToList(), keyshift).ToArray();
            int maxLength = Math.Max(valueB.Length, keyB.Length);
            List<byte> ExitByte = new List<byte>();
            ExitByte = ByteXor(maxLength, valueB, keyB);
            ExitByte = CaesarCipher(ExitByte, shift);

            byte[] byteArray = ExitByte.ToArray();


            
            string filenew = MainWindow.SaveFileName();
            File.WriteAllBytes(filenew, byteArray);

        }
        private static List<byte> ByteXor(int maxLength, byte[] valueB, byte[] keyB)
        {
            List<byte> ExitByte = new List<byte>();
            for (int i = 0; i < maxLength; i++)
            {
                byte byte1 = valueB[i % valueB.Length];
                byte byte2 = keyB[i % keyB.Length];
                byte byte3 = (byte)(byte1 ^ byte2);
                ExitByte.Add(byte3);

            }
            return ExitByte;
        }
        public static void Decript(string value, int shift, string key, int keyshift)
        {

            byte[] byteArray = File.ReadAllBytes(value);


            byte[] keyB = ConvetByte(key);
            keyB = CaesarCipher(keyB.ToList(), keyshift).ToArray();


            List<byte> shiftedBytes = CaesarCipher(byteArray.ToList(), -shift);


            int maxLength = Math.Max(shiftedBytes.Count, keyB.Length);
            List<byte> decryptedBytes = ByteXor(maxLength, shiftedBytes.ToArray(), keyB);


            byte[] byteArrayExit = decryptedBytes.ToArray();
            byteArrayExit = TripleDesEncryption.Decrypt(byteArrayExit, key);
            byteArrayExit = AesEncryption.Decrypt(byteArrayExit, key);


            string filenew = MainWindow.SaveFileName();
            File.WriteAllBytes(filenew, byteArrayExit);
        }
        private static byte[] ConvetByte(string convert)
        {
            return Encoding.UTF8.GetBytes(convert);
        }
        static List<byte> CaesarCipher(List<byte> input, int shift)
        {
            List<byte> output = new List<byte>();
            foreach (byte b in input)
            {
                int shiftedByte = b + shift;

                if (shiftedByte > 255)
                {
                    shiftedByte -= 256;
                }
                else if (shiftedByte < 0)
                {
                    shiftedByte += 256;
                }

                output.Add((byte)shiftedByte);
            }
            return output;
        }
    }
}
