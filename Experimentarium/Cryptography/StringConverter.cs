using System;
using System.Globalization;
using System.Text;

namespace Experimentarium.Cryptography
{
    public class StringConverter
    {
        public static byte[] ConvertHexStringToBytes(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Number of chars in hex string must be even");
            }

            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0, j = 0; i < hexString.Length; i += 2, j++)
            {
                string hexSubstring = hexString.Substring(i, 2);
                byte b;
                if (Byte.TryParse(hexSubstring, NumberStyles.HexNumber, null, out b))
                {
                    bytes[j] = b;
                }
                else
                {
                    throw new ArgumentException($"HexString contains non-hex value: {hexSubstring}");
                }
            }

            return bytes;
        }

        public static string ConvertBytesToHexString(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length);

            foreach (byte b in bytes)
            {
                result.Append(b.ToString("x2"));
            }

            return result.ToString();
        }

        public static string ConvertHexStringToAsciiString(string hexString)
        {
            return Encoding.ASCII.GetString(ConvertHexStringToBytes(hexString));
        }

        public static string ConvertBytesToAsciiString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                string s = Encoding.ASCII.GetString(bytes, i, 1);
                if ((String.CompareOrdinal(s, "A") >= 0 && String.CompareOrdinal(s, "Z") <= 0) || 
                    (String.CompareOrdinal(s, "a") >= 0 && String.CompareOrdinal(s, "z") <= 0))
                {
                    result.Append(s);
                }
                else
                {
                    result.Append("_");
                }
            }

            return result.ToString();
        }
    }
}