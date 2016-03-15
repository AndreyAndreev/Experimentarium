using System.Security.Cryptography;

namespace Experimentarium.Cryptography
{
    public class DoubleSha256
    {
        public static byte[] Hash(byte[] bytes)
        {
            return Hash(bytes, 0, bytes.Length);
        }

        public static byte[] Hash(byte[] bytes, int offset, int count)
        {
            using (var x = new SHA256Managed())
            {
                var hash1 = x.ComputeHash(bytes, offset, count);
                var hash2 = x.ComputeHash(hash1);
                return hash2;
            }
        }
    }
}