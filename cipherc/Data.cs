/*
 * author: XUJINKAI, https://github.com/XUJINKAI
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.Exceptions;

#pragma warning disable CA1050 // 在命名空间中声明类型
#pragma warning disable CA2225 // 运算符重载具有命名的备用项

/// <summary>
/// 自定义数据类，可由string或bytes隐式转换，方便函数入参
/// </summary>
public readonly struct Data
{
    private readonly byte[] _bytes;
    public int Length => _bytes.Length;
    public byte[] GetBytes() => _bytes;
    public byte[] GetBytesCopy() => _bytes.DeepCopy();

    #region Constructor

    private Data(byte[] byteArray) => _bytes = byteArray;

    public static Data FromBytes(byte[] bytes) => new Data(bytes);
    public static Data FromBytesCopy(byte[] bytes) => new Data(bytes.DeepCopy());

    public static Data FromHexString(string hexString) => new Data(FromHexStringToByteArray(hexString));
    public static Data FromBinaryString(string binString) => new Data(FromBinaryStringToByteArray(binString));
    public static Data FromBase64String(string base64) => new Data(FromBase64StringToByteArray(base64));
    public static Data FromUtf8String(string utf8String) => new Data(utf8String.GetBytes("UTF8"));
    public static Data FromAsciiString(string asciiString) => new Data(asciiString.GetBytes("ASCII"));
    public static Data FromFile(string path) => new Data(File.ReadAllBytes(path));

    #endregion

    #region Output

    public string ToHexString(string sep = "") => _bytes.ToHexString(sep);
    public string ToBinaryString(string sep = "") => _bytes.ToBinaryString(sep);
    public string ToBase64String() => _bytes.ToBase64String();
    public string ToUtf8String() => _bytes.ToUtf8String();
    public string ToAsciiString() => _bytes.ToAsciiString();
    public string ToFormatHexText() => _bytes.ToFormatHexText();
    public void SaveToFile(string path) => File.WriteAllBytes(path, _bytes);

    #endregion

    #region implicit/operator/override

    public static implicit operator byte[](Data data) => data._bytes;
    public static implicit operator Data(byte[] byteArray) => new Data(byteArray);
    public static implicit operator Data(string utf8String) => new Data(utf8String.GetBytes("UTF8"));

    public static bool operator ==(Data data1, Data data2) => data1.Equals(data2);
    public static bool operator !=(Data data1, Data data2) => !data1.Equals(data2);
    public static bool operator ==(Data data, byte[] bytes) => data.Equals(bytes);
    public static bool operator !=(Data data, byte[] bytes) => !data.Equals(bytes);
    public static bool operator ==(Data data, string str) => data.Equals(str);
    public static bool operator !=(Data data, string str) => !data.Equals(str);

    public static Data operator +(Data data1, Data data2) => data1.Concat(data2);
    public static Data operator *(Data data1, int times) => data1.Repeat(times);

    public override string ToString() => ToFormatHexText();
    public override int GetHashCode() => _bytes.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Data data => _bytes.SequenceEqual(data._bytes),
            byte[] bytes => _bytes.SequenceEqual(bytes),
            string str => _bytes.SequenceEqual(str.GetBytes("UTF8")),
            _ => false,
        };
    }

    #endregion

    #region slice

    public byte this[int index]
    {
        get => _bytes[index];
        set => _bytes[index] = value;
    }
    public byte this[Index index]
    {
        get => _bytes[index];
        set => _bytes[index] = value;
    }
    public byte[] this[Range range]
    {
        get => _bytes[range];
    }
    public Data Sub(int start, int length) => _bytes.SubArray(start, length);

    #endregion

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
        if (hexStr.Length % 2 != 0) hexStr = "0" + hexStr;

        int numberChars = hexStr.Length;
        byte[] bytes = new byte[numberChars / 2];
        try
        {
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexStr.Substring(i, 2), 16);
        }
        catch (FormatException formatException)
        {
            throw new DecodeException("hex", hexStr, formatException);
        }
        return bytes;
    }

    /// <summary>
    /// valid input: 1100, 11110000
    /// </summary>
    /// <param name="binStr"></param>
    /// <returns></returns>
    public static byte[] FromBinaryStringToByteArray(string binStr)
    {
        var input = binStr.PadLeft((binStr.Length + 7) / 8 * 8, '0');
        int numOfBytes = input.Length / 8;
        byte[] bytes = new byte[numOfBytes];
        try
        {
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
            }
        }
        catch (FormatException formatException)
        {
            throw new DecodeException("bin", binStr, formatException);
        }
        return bytes;
    }

    public static byte[] FromBase64StringToByteArray(string base64Str)
    {
        try
        {
            return Convert.FromBase64String(base64Str);
        }
        catch (FormatException formatException)
        {
            throw new DecodeException("base64", base64Str, formatException);
        }
    }

    #endregion
}

public static class Extensions
{
    public static Data GetData(this byte[] bytes) => Data.FromBytes(bytes);

    public static byte[] DeepCopy(this byte[] bytes)
    {
        var copy = new byte[bytes.Length];
        Buffer.BlockCopy(bytes, 0, copy, 0, bytes.Length);
        return copy;
    }

    /// <summary>
    /// 将字符串中的每一个字符都转换成对应的byte，对于Unicode字符，默认使用UTF8编码
    /// </summary>
    /// <param name="s"></param>
    /// <param name="encoding">Available value: ASCII, UTF8, UTF16, UTF32。</param>
    /// <returns></returns>
    public static byte[] GetBytes(this string s, string encoding = "UTF8")
    {
        return encoding switch
        {
            "ASCII" => Encoding.ASCII.GetBytes(s),
            "UTF8" => Encoding.UTF8.GetBytes(s),
            "UTF16" => Encoding.Unicode.GetBytes(s),
            "UTF32" => Encoding.UTF32.GetBytes(s),
            _ => throw new ArgumentException($"Unknown encoding {encoding}"),
        };
    }

    public static string JoinToString(this IEnumerable<string> array, string seperator) => string.Join(seperator, array);

    #region array

    /// <summary>
    /// 与System.Linq.Enumerable.SequenceEqual的区别：参数可为null
    /// </summary>
    public static bool SequenceEqual<T>(this IEnumerable<T>? array, IEnumerable<T>? compare)
    {
        if (array == null || compare == null)
            return array == compare;
        return Enumerable.SequenceEqual(array, compare);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
            action(item);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
    {
        int i = 0;
        foreach (T item in source)
            action(i++, item);
    }

    public static IEnumerable<T[]> SplitBySize<T>(this T[] array, int maxChunkSize)
    {
        for (int i = 0; i < array.Length; i += maxChunkSize)
            yield return array.SubArrayMaxLength(i, maxChunkSize);
    }

    public static IEnumerable<string> SplitBySize(this string s, int maxChunkSize)
    {
        for (int i = 0; i < s.Length; i += maxChunkSize)
            yield return s.Substring(i, Math.Min(maxChunkSize, s.Length - i));
    }

    /// <summary>
    /// length > 0  : SubArray(index, length)
    /// length == 0 : SubArray(index)
    /// length < 0  : SubArray(index, -length), and not throw if out of range
    /// </summary>
    public static T[] SubArrayEx<T>(this T[] array, int index, int length)
    {
        return length switch
        {
            > 0 => array.SubArray(index, length),
            0 => array.SubArray(index),
            < 0 => array.SubArrayMaxLength(index, -length),
        };
    }

    public static T[] SubArray<T>(this T[] array, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, index, result, 0, length);
        return result;
    }

    public static T[] SubArray<T>(this T[] array, int index)
    {
        T[] result = new T[array.Length - index];
        Array.Copy(array, index, result, 0, result.Length);
        return result;
    }

    public static T[] SubArrayMaxLength<T>(this T[] array, int index, int maxLength)
    {
        int len = Math.Min(maxLength, array.Length - index);
        T[] result = new T[len];
        Array.Copy(array, index, result, 0, len);
        return result;
    }

    public static T[] SubFirst<T>(this T[] array, int len)
    {
        T[] result = new T[len];
        Array.Copy(array, 0, result, 0, len);
        return result;
    }

    public static byte[] Concat(this byte[] array1, byte[] array2)
    {
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

    public static string Repeat(this string str, int times) => string.Concat(Enumerable.Repeat(str, times));

    #endregion

    #region print string

    public static bool IsPrintable(this char ch) => ch >= 32 && ch <= 126;

    public static bool IsPrintable(this byte b) => b >= 32 && b <= 126;

    public static bool IsPrintable(this byte[] bytes) => bytes.All(b => b.IsPrintable());

    public static string ToHexString(this byte[] bytes, string sep = "") => BitConverter.ToString(bytes).Replace("-", sep, StringComparison.Ordinal);

    public static string ToHexString(this int number, int paddingLength = 8) => number.ToString("X").PadLeft(paddingLength, '0');

    public static string ToBinaryString(this byte b) => Convert.ToString(b, 2).PadLeft(8, '0');

    public static string ToBinaryString(this byte[] bytes, string sep = "") => string.Join(sep, bytes.Select(b => b.ToBinaryString()));

    public static string ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

    /// <summary>
    /// 注意：并非所有Byte都是可读字符。
    /// </summary>
    public static string ToAsciiString(this byte[] bytes) => Encoding.ASCII.GetString(bytes);

    public static string ToUtf8String(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

    public static string ToFormatHexText(this byte[] bytes)
    {
        const int BlockSize = 16;
        int hex_line_length = BlockSize * 2 + BlockSize - 1;
        var sb = new StringBuilder();
        bytes.SplitBySize(BlockSize).ForEach((idx, block) =>
        {
            var pos = (idx * BlockSize).ToHexString(8);
            var hex = block.ToHexString().SplitBySize(2).JoinToString(" ");
            var pad = new string(' ', hex_line_length - hex.Length);
            var ascii = block.Select(c => c.IsPrintable() ? c : (byte)'.').ToArray().ToAsciiString();
            sb.AppendLine($"0x{pos}    {hex} {pad}   {ascii}");
        });
        return sb.ToString();
    }

    #endregion
}
