using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace libcipherc.Crypto
{
    public static class Sym
    {
        private static byte[] CBC(string algrName, int keySize,
            IBlockCipher __engine, IBlockCipherPadding? __padding, bool __enc,
            byte[] data, byte[] key, byte[] iv)
        {
            Parameter.CheckLength(key, $"{algrName} Key", keySize);
            Parameter.CheckLength(iv, $"{algrName} IV", keySize);

            var modeCipher = new CbcBlockCipher(__engine);
            IBufferedCipher cipher = __padding == null
                ? new BufferedBlockCipher(modeCipher)
                : new PaddedBufferedBlockCipher(modeCipher, __padding);

            ICipherParameters keyParam = new KeyParameter(key);
            keyParam = new ParametersWithIV(keyParam, iv);

            cipher.Init(__enc, keyParam);
            var outputSize = cipher.GetOutputSize(data.Length);
            var output = new byte[outputSize];
            int length = cipher.ProcessBytes(data, output, 0);
            int finalBlockLenght = cipher.DoFinal(output, length);

            return __enc ? output : output[..^(length - finalBlockLenght)];
        }


        private static byte[] ECB(string algrName, int keySize,
            IBlockCipher __engine, IBlockCipherPadding? __padding, bool __enc,
            byte[] data, byte[] key)
        {
            Parameter.CheckLength(key, $"{algrName} Key", keySize);

            var modeCipher = __engine;
            IBufferedCipher cipher = __padding == null
                ? new BufferedBlockCipher(modeCipher)
                : new PaddedBufferedBlockCipher(modeCipher, __padding);

            ICipherParameters keyParam = new KeyParameter(key);

            cipher.Init(__enc, keyParam);
            var outputSize = cipher.GetOutputSize(data.Length);
            var output = new byte[outputSize];
            int length = cipher.ProcessBytes(data, output, 0);
            int finalBlockLenght = cipher.DoFinal(output, length);

            return __enc ? output : output[..^(length - finalBlockLenght)];
        }


        public static byte[] SM4_CBC_Enc(byte[] data, byte[] key, byte[] iv)
        {
            return CBC("SM4", 16, new SM4Engine(), new Pkcs7Padding(), true,
                data, key, iv);
        }

        public static byte[] SM4_CBC_Dec(byte[] data, byte[] key, byte[] iv)
        {
            return CBC("SM4", 16, new SM4Engine(), new Pkcs7Padding(), false,
                data, key, iv);
        }

        public static byte[] SM4_ECB_Enc(byte[] data, byte[] key)
        {
            return ECB("SM4", 16, new SM4Engine(), new Pkcs7Padding(), true,
                data, key);
        }

        public static byte[] SM4_ECB_Dec(byte[] data, byte[] key)
        {
            return ECB("SM4", 16, new SM4Engine(), new Pkcs7Padding(), false,
                data, key);
        }


        public static byte[] AES128_CBC_Enc(byte[] data, byte[] key, byte[] iv)
        {
            return CBC("AES", 16, new AesEngine(), new Pkcs7Padding(), true,
                data, key, iv);
        }

        public static byte[] AES128_CBC_Dec(byte[] data, byte[] key, byte[] iv)
        {
            return CBC("AES", 16, new AesEngine(), new Pkcs7Padding(), false,
                data, key, iv);
        }

        public static byte[] AES128_ECB_Enc(byte[] data, byte[] key)
        {
            return ECB("AES", 16, new AesEngine(), new Pkcs7Padding(), true,
                data, key);
        }

        public static byte[] AES128_ECB_Dec(byte[] data, byte[] key)
        {
            return ECB("AES", 16, new AesEngine(), new Pkcs7Padding(), false,
                data, key);
        }
    }
}
