using libcipherc.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Handler
{
    public static class DATA
    {
        private static Func<int, byte[]> randGenerator = Rand.RandomBytes;

        public static Func<int, byte[]> RandGenerator
        {
            get => randGenerator;
#if DEBUG
            set => randGenerator = value;
#endif
        }

        public static byte[] RAND(int bytes)
        {
            return RandGenerator(bytes);
        }

        public static byte[] RAND(string s)
        {
            try
            {
                return RAND(int.Parse(s));
            }
            catch (FormatException ex)
            {
                throw new ArgumentErrorException($"Input is not integer: {s}", ex);
            }
        }


        public static byte[] ZEROS(int bytes)
        {
            return new byte[bytes];
        }

        public static byte[] ZEROS(string s)
        {
            try
            {
                return ZEROS(int.Parse(s));
            }
            catch (FormatException ex)
            {
                throw new ArgumentErrorException($"Input is not integer: {s}", ex);
            }
        }


        public static byte[] BASE64(string base64)
        {
            try
            {
                return BytesExtension.FromBase64StringToByteArray(base64.ToSingleLine());
            }
            catch (FormatException ex)
            {
                throw new ArgumentErrorException($"Input is not base64: {base64}", ex);
            }
        }

        public static byte[] HEX(string hex)
        {
            try
            {
                return BytesExtension.FromHexStringToByteArray(hex.Replace("\\x", ""));
            }
            catch (FormatException ex)
            {
                throw new ArgumentErrorException($"Input is not hex: {hex}", ex);
            }
        }

        public static byte[] FILE(string path)
        {
            if (!File.Exists(path))
            {
                throw new LoadFileException($"File not exist: {path}");
            }
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                throw new LoadFileException($"Cannot load file: {path}", ex);
            }
        }
    }
}
