using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CipherTool.Transform
{
    public static class ByteArrayExtension
    {
        public static byte[]? DeepCopy(this byte[]? array)
        {
            if (array == null) return null;
            var copy = new byte[array.Length];
            Buffer.BlockCopy(array, 0, copy, 0, array.Length);
            return copy;
        }

        public static bool IsPrintable(this char ch)
        {
            return ch >= 32 && ch <= 126;
        }

        public static bool IsPrintable(this byte b)
        {
            return b >= 32 && b <= 126;
        }

        public static bool IsPrintable(this byte[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return array.All(b => b.IsPrintable());
        }

        /// <summary>
        /// 注意：并非所有Byte都是可读字符。此转换仅用于查看，再转回去不一定是原文。
        /// </summary>
        /// <returns></returns>
        public static string ToAsciiString(this byte[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (!array.IsPrintable()) Console.Error.WriteLine("[WARN] Not all bytes are printable.");
            return Encoding.ASCII.GetString(array);
        }

        public static string ToHexString(this byte[] array, string sep = "")
        {
            return BitConverter.ToString(array).Replace("-", sep, StringComparison.Ordinal);
        }

        public static string ToBase64String(this byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        public static byte[] Concat(this byte[] array1, byte[] array2)
        {
            if (array1 == null) throw new ArgumentNullException(nameof(array1));
            if (array2 == null) throw new ArgumentNullException(nameof(array2));

            byte[] result = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, result, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
    }
}
