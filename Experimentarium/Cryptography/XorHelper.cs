using System;

namespace Experimentarium.Cryptography
{
    public class XorHelper
    {
        public static string XorHexStrings(string first, string second)
        {
            byte[] xoredBytes = XorHexStringsToBytes(first, second);

            string xoredString = StringConverter.ConvertBytesToHexString(xoredBytes);

            return xoredString;
        }

        public static byte[] XorHexStringsToBytes(string first, string second)
        {
            if (first.Length != second.Length)
            {
                throw new ArgumentException("Both strings must have same size");
            }

            byte[] firstBytes = StringConverter.ConvertHexStringToBytes(first);
            byte[] secondBytes = StringConverter.ConvertHexStringToBytes(second);

            byte[] xoredBytes = new byte[firstBytes.Length];

            for (int i = 0; i < firstBytes.Length; i++)
            {
                byte xoredByte = (byte) (firstBytes[i] ^ secondBytes[i]);
                xoredBytes[i] = xoredByte;
            }

            return xoredBytes;
        }
    }
}