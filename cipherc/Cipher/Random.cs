using System.Security.Cryptography;

namespace CipherTool.Cipher
{
    public static class Random
    {
        public static byte[] RandomBytes(int nbytes)
        {
            byte[] result = new byte[nbytes];
            RandomNumberGenerator.Fill(result);
            return result;
        }
    }
}
