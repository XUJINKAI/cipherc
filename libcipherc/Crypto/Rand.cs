using System.Security.Cryptography;

namespace libcipherc.Crypto
{
    public static class Rand
    {
        public static byte[] RandomBytes(int bytes)
        {
            byte[] result = new byte[bytes];
            RandomNumberGenerator.Fill(result);
            return result;
        }
    }
}
