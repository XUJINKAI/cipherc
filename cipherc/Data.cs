using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.Exceptions;

#pragma warning disable CA2225 // 运算符重载具有命名的备用项

namespace CipherTool
{
    /// <summary>
    /// 自定义数据类，可由string或bytes隐式转换，方便函数入参
    /// </summary>
    public readonly struct Data : IEquatable<Data>, IEquatable<byte[]>
    {
        private readonly byte[] _bytes;
        public int Length => _bytes.Length;
        public byte[] GetBytes() => _bytes.DeepCopy() ?? throw new Exception();

        #region Constructor

        /// <summary>
        /// 使用UTF8编码字符串初始化
        /// </summary>
        /// <param name="plainString"></param>
        public Data(string plainString) => _bytes = plainString.GetBytes();
        public Data(byte[] byteArray) => _bytes = byteArray;

        public static Data FromBytes(byte[] bytes) => new Data(bytes);
        public static Data FromUtf8String(string plainString) => new Data(plainString.GetBytes("UTF8"));
        public static Data FromAsciiString(string plainString) => new Data(plainString.GetBytes("ASCII"));
        public static Data FromBase64String(string base64) => new Data(FromBase64StringToByteArray(base64));
        public static Data FromHexString(string hexString) => new Data(FromHexStringToByteArray(hexString));
        public static Data FromBinaryString(string binString) => new Data(FromBinaryStringToByteArray(binString));
        public static Data FromFile(string path) => new Data(File.ReadAllBytes(path));

        #endregion

        #region implicit convert

        public static implicit operator byte[](Data data) => data._bytes;
        public static implicit operator Data(byte[] byteArray) => new Data(byteArray);
        public static implicit operator Data(string utf8String) => new Data(utf8String.GetBytes("UTF8"));

        #endregion

        #region Output

        public string ToAsciiString() => FromBytesToAsciiString(_bytes);
        public string ToUtf8String() => FromBytesToUtf8String(_bytes);
        public string ToBase64String() => _bytes.ToBase64String();
        public string ToHexString(string sep = "") => _bytes.ToHexString(sep);
        public string ToBinaryString(string sep = "") => _bytes.ToBinaryString(sep);
        public void SaveToFile(string path) => File.WriteAllBytes(path, _bytes);

        #endregion

        public static bool operator ==(Data data1, Data data2) => data1.Equals(data2);
        public static bool operator !=(Data data1, Data data2) => !data1.Equals(data2);
        public static bool operator ==(Data data, string str) => data.Equals(str);
        public static bool operator !=(Data data, string str) => !data.Equals(str);
        public static bool operator ==(Data data, byte[] bytes) => data.Equals(bytes);
        public static bool operator !=(Data data, byte[] bytes) => !data.Equals(bytes);

        public static Data operator +(Data data1, Data data2) => data1.Concat(data2);
        public static Data operator *(Data data1, int times) => data1.Repeat(times);

        /// <summary>
        /// Encoding to Hex String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToHexString("");
        public override int GetHashCode() => _bytes.GetHashCode();

        public bool Equals(Data other) => _bytes.SequenceEqual(other._bytes);

        public bool Equals(byte[]? other) => _bytes.SequenceEqual(other);

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Data data => _bytes.SequenceEqual(data._bytes),
                byte[] bytes => _bytes.SequenceEqual(bytes),
                _ => false,
            };
        }

        public Data Sub(int start, int length) => _bytes.SubArray(start, length);
        public Data Concat(Data data) => _bytes.Concat(data._bytes);
        public Data Repeat(int times) => _bytes.Repeat(times);
        public bool IsPrintable() => _bytes.IsPrintable();

        #region Static Method

        /// <summary>
        /// 调用字符串为16进制编码字串，将此字串还原为bytes。
        /// 此函数为byte[].ToHexString()的反函数。
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] FromHexStringToByteArray(string hexStr)
        {
            if (hexStr == null) throw new ArgumentNullException(nameof(hexStr));
            if (hexStr.Length % 2 != 0) hexStr = "0" + hexStr;

            try
            {
                int numberChars = hexStr.Length;
                byte[] bytes = new byte[numberChars / 2];
                for (int i = 0; i < numberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hexStr.Substring(i, 2), 16);
                return bytes;
            }
            catch (FormatException exp)
            {
                throw new DecodeException("hex", hexStr, exp);
            }
        }

        /// <summary>
        /// valid input: 1100, 11110000
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static byte[] FromBinaryStringToByteArray(string binStr)
        {
            if (binStr == null) throw new ArgumentNullException(nameof(binStr));

            try
            {
                var input = binStr.PadLeft((binStr.Length + 7) / 8 * 8, '0');
                int numOfBytes = input.Length / 8;
                byte[] bytes = new byte[numOfBytes];
                for (int i = 0; i < numOfBytes; ++i)
                {
                    bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
                }
                return bytes;
            }
            catch (FormatException exp)
            {
                throw new DecodeException("binary", binStr, exp);
            }
        }

        public static byte[] FromBase64StringToByteArray(string base64Str)
        {
            try
            {
                return Convert.FromBase64String(base64Str);
            }
            catch (FormatException exp)
            {
                throw new DecodeException("base64", base64Str, exp);
            }
        }

        /// <summary>
        /// 注意：并非所有Byte都是可读字符。
        /// </summary>
        /// <returns></returns>
        public static string FromBytesToAsciiString(byte[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return Encoding.ASCII.GetString(array);
        }

        public static string FromBytesToUtf8String(byte[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return Encoding.UTF8.GetString(array);
        }

        #endregion
    }

    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (T item in source)
            {
                action(item);
            }
        }
    }

    public static class ArrayExtension
    {
        public static IEnumerable<T[]> SplitBySize<T>(this T[] array, int maxChunkSize)
        {
            for (int i = 0; i < array.Length; i += maxChunkSize)
                yield return array.SubArray(i, Math.Min(maxChunkSize, array.Length - i));
        }

        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            T[] result = new T[length];
            Array.Copy(array, index, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(this T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            T[] result = new T[array.Length - index];
            Array.Copy(array, index, result, 0, result.Length);
            return result;
        }

        public static T[] SubFirst<T>(this T[] array, int len)
        {
            T[] result = new T[len];
            Array.Copy(array, 0, result, 0, len);
            return result;
        }

        public static bool SequenceEqual<T>(this T[]? array, T[]? compare)
        {
            if (array == null || compare == null)
                return array == compare;
            return Enumerable.SequenceEqual(array, compare);
        }
    }

    public static class ByteArrayExtension
    {
        public static Data GetData(this byte[] array)
        {
            return Data.FromBytes(array);
        }

        public static byte[]? DeepCopy(this byte[]? array)
        {
            if (array == null) return null;
            var copy = new byte[array.Length];
            Buffer.BlockCopy(array, 0, copy, 0, array.Length);
            return copy;
        }

        public static bool IsPrintable(this byte[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return array.All(b => b.IsPrintable());
        }

        public static string ToHexString(this byte[] array, string sep = "")
        {
            return BitConverter.ToString(array).Replace("-", sep, StringComparison.Ordinal);
        }

        public static string ToBase64String(this byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        public static string ToBinaryString(this byte[] array, string sep = "")
        {
            return string.Join(sep, array.Select(b => b.ToBinaryString()));
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

        public static byte[] Repeat(this byte[] array, int times)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            var copy = new byte[array.Length * times];
            for (int i = 0; i < times; i++)
            {
                Buffer.BlockCopy(array, 0, copy, i * array.Length, array.Length);
            }

            return copy;
        }
    }

    public static class CharByteExtension
    {
        public static bool IsPrintable(this char ch)
        {
            return ch >= 32 && ch <= 126;
        }

        public static bool IsPrintable(this byte b)
        {
            return b >= 32 && b <= 126;
        }

        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// 将字符串中的每一个字符都转换成对应的byte，对于Unicode字符，默认使用UTF8编码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding">Available value: ASCII, UTF8, UTF16, UTF32。</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string s, string? encoding = "UTF8")
        {
            return encoding switch
            {
                "UTF8" => Encoding.UTF8.GetBytes(s),
                "ASCII" => Encoding.ASCII.GetBytes(s),
                "UTF16" => Encoding.Unicode.GetBytes(s),
                "UTF32" => Encoding.UTF32.GetBytes(s),
                _ => throw new ArgumentException($"Unknown encoding {encoding}"),
            };
        }

        public static string Repeat(this string str, int times)
        {
            return string.Concat(Enumerable.Repeat(str, times));
        }

        public static IEnumerable<string> SplitBySize(this string s, int maxChunkSize)
        {
            for (int i = 0; i < s.Length; i += maxChunkSize)
                yield return s.Substring(i, Math.Min(maxChunkSize, s.Length - i));
        }
    }

    public static class StringArrayExtension
    {
        public static string JoinToString(this IEnumerable<string> array, string seperator) => string.Join(seperator, array);
    }
}
