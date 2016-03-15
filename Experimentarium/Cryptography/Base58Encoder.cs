using System;
using System.Diagnostics;
using System.Numerics;

namespace Experimentarium.Cryptography
{
    public class Base58Encoder
    {
        private static readonly char[] Chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".ToCharArray();

        private static readonly BigInteger Base = new BigInteger(58);

        public static byte[] DecodeBase58(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (input.Length == 0) return new byte[0];

            var x = new BigInteger();

            for (var i = 0; i < input.Length; i++)
            {
                var index = Array.IndexOf(Chars, input[i]);

                if (index < 0)
                {
                    var msg = string.Format("Illegal character {0} at {1}", input[i], i);
                    throw new ArgumentException(msg);
                }

                x = x * Base;
                x += index;
            }

            Debug.Assert(x.Sign >= 0);

            var bytes = x.ToByteArray();
            var bytesLength = bytes.Length;

            while (bytesLength > 0 && bytes[bytesLength - 1] == 0)
                --bytesLength;

            var leadingZeros = 0;
            for (var i = 0; i < input.Length && input[i] == Chars[0]; i++)
                leadingZeros++;

            var tmp = new byte[bytesLength + leadingZeros];

            for (var i = 0; i < bytesLength; i++)
            {
                tmp[bytesLength - i - 1 + leadingZeros] = bytes[i];
            }

            return tmp;
        }
    }
}