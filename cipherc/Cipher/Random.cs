using System.Security.Cryptography;

namespace CipherTool.Cipher
{
    public static class Random
    {
        public static byte[] RandomBytes(int bytes)
        {
            byte[] result = new byte[bytes];
            RandomNumberGenerator.Fill(result);
            return result;
        }
    }
}
