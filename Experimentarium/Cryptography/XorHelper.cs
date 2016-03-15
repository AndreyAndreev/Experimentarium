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
            byte[] firstBytes = StringConverter.ConvertHexStringToBytes(first);
            byte[] secondBytes = StringConverter.ConvertHexStringToBytes(second);

            byte[] xoredBytes = new byte[firstBytes.Length];

            int bound = Math.Min(firstBytes.Length, secondBytes.Length);

            for (int i = 0; i < bound; i++)
            {
                byte xoredByte = (byte) (firstBytes[i] ^ secondBytes[i]);
                xoredBytes[i] = xoredByte;
            }

            return xoredBytes;
        }
    }
}