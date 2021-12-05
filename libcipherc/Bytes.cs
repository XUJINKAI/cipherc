/*
 * author: XUJINKAI, https://github.com/XUJINKAI
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace libcipherc;


public interface IBytes
{
    byte[] GetBytes();
}

/// <summary>
/// 对byte[]类型的封装，支持从各种格式转入转出
/// </summary>
public class Bytes : IBytes
{
    public const string DefaultEncoding = "UTF8";

    public virtual byte[] ByteArray { get; set; }
    public int Length => ByteArray.Length;

    public Bytes(byte[] byteArray)
    {
        ByteArray = byteArray;
    }

    public byte[] GetBytes() => ByteArray;

    public static Bytes FromBytes(byte[] bytes) => new Bytes(bytes);
    public static Bytes FromHexString(string hexString) => new Bytes(BytesExtension.FromHexStringToByteArray(hexString));
    public static Bytes FromBinaryString(string binString) => new Bytes(BytesExtension.FromBinaryStringToByteArray(binString));
    public static Bytes FromBase64String(string base64) => new Bytes(BytesExtension.FromBase64StringToByteArray(base64));
    public static Bytes FromUtf8String(string utf8String) => new Bytes(utf8String.GetBytes("UTF8"));
    public static Bytes FromFile(string path) => new Bytes(File.ReadAllBytes(path));

    #region Output

    public string ToHexString(string sep = "") => ByteArray.ToHexString(sep);
    public string ToBinaryString(string sep = "") => ByteArray.ToBinaryString(sep);
    public string ToBase64String(string eol = "") => ByteArray.ToBase64String(eol);
    public string ToUtf8String() => ByteArray.ToUtf8String();
    public string ToAsciiString() => ByteArray.ToAsciiString();
    public string ToHexDumpText(string? eol = null) => ByteArray.ToHexDumpText(eol);
    public void ToWriteFile(string path) => File.WriteAllBytes(path, ByteArray);

    #endregion

    #region implicit/operator/override

    public static implicit operator byte[](Bytes data) => data.ByteArray;
    public static implicit operator Bytes(byte[] byteArray) => new Bytes(byteArray);
    public static implicit operator Bytes(string _string) => new Bytes(_string.GetBytes(DefaultEncoding));

    public static bool operator ==(Bytes data1, Bytes data2) => data1.Equals(data2);
    public static bool operator !=(Bytes data1, Bytes data2) => !data1.Equals(data2);
    public static bool operator ==(Bytes data, byte[] bytes) => data.Equals(bytes);
    public static bool operator !=(Bytes data, byte[] bytes) => !data.Equals(bytes);
    public static bool operator ==(Bytes data, string str) => data.Equals(str);
    public static bool operator !=(Bytes data, string str) => !data.Equals(str);

    public override string ToString() => this.ToHexString();
    public override int GetHashCode() => ByteArray.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Bytes data => ByteArray.SequenceEqual(data.ByteArray),
            byte[] bytes => ByteArray.SequenceEqual(bytes),
            string str => ByteArray.SequenceEqual(str.GetBytes(DefaultEncoding)),
            _ => false,
        };
    }

    #endregion

    #region slice

    public byte this[int index]
    {
        get => ByteArray[index];
        set => ByteArray[index] = value;
    }
    public byte this[Index index]
    {
        get => ByteArray[index];
        set => ByteArray[index] = value;
    }
    public byte[] this[Range range]
    {
        get => ByteArray[range];
    }

    #endregion
}


public static class StringExtension
{
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

    public static byte[] GetBytes(this string s, Encoding encoding) => encoding.GetBytes(s);

    public static string JoinToString(this IEnumerable<string> array, string seperator)
    {
        return string.Join(seperator, array);
    }

    public static string Repeat(this string str, int times)
    {
        return string.Concat(Enumerable.Repeat(str, times));
    }

    public static string TrimEnd(this string s, string needle)
    {
        return s.EndsWith(needle) ? s[..^needle.Length] : s;
    }

    /// <summary>
    /// Remove all LF and CR in string
    /// </summary>
    public static string ToSingleLine(this string s)
    {
        return s.Replace("\n", "").Replace("\r", "");
    }

    public static string[] SplitToLines(this string s)
    {
        return s.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

}


public static class ArrayExtension
{
    /// <summary>
    /// 与System.Linq.Enumerable.SequenceEqual的区别：参数可为null
    /// </summary>
    public static bool SequenceEqual<T>(this IEnumerable<T>? array, IEnumerable<T>? compare)
    {
        if (array == null || compare == null)
            return array == compare;
        return Enumerable.SequenceEqual(array, compare);
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
}


public static class BytesExtension
{
    #region string to bytes

    /// <summary>
    /// 将16进制字符串(HEX)还原为bytes。
    /// </summary>
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
            throw new FormatException($"string {hexStr} is not valid hex string.", formatException);
        }
        return bytes;
    }

    /// <summary>
    /// 将2进制字符串(BIN)还原为bytes。
    /// </summary>
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
            throw new FormatException($"string {binStr} is not valid binary string.", formatException);
        }
        return bytes;
    }

    /// <summary>
    /// 将Base64字符串还原为bytes。
    /// </summary>
    public static byte[] FromBase64StringToByteArray(string base64Str)
    {
        try
        {
            return Convert.FromBase64String(base64Str);
        }
        catch (FormatException formatException)
        {
            throw new FormatException($"string {base64Str} is not valid base64 string.", formatException);
        }
    }

    #endregion

    #region bytes to string

    public static string ToHexString(this byte[] bytes, string sep = "") => BitConverter.ToString(bytes).Replace("-", sep, StringComparison.Ordinal);
    public static string ToHexString(this int number, int paddingLength = 8) => number.ToString("X").PadLeft(paddingLength, '0');
    public static string ToHexDumpText(this byte[] bytes, string? eol = null)
    {
        const int BlockSize = 16;
        eol ??= Environment.NewLine;
        var splits = bytes.SplitBySize(BlockSize);
        var sb = new StringBuilder();
        int idx = 0;
        foreach (var block in splits)
        {
            var pos = (idx * BlockSize).ToHexString(8);
            var hex = block.ToHexString(" ").SplitBySize(8 * 3).JoinToString(" ");
            var pad = new string(' ', BlockSize * 3 - hex.Length);
            var ascii = block.Select(c => c.IsPrintableAscii() ? c : (byte)'.').ToArray().ToAsciiString();
            sb.Append($"0x{pos}  {hex}{pad}  {ascii}{eol}");
            idx++;
        }
        return sb.ToString();
    }

    public static string ToBinaryString(this byte b) => Convert.ToString(b, 2).PadLeft(8, '0');
    public static string ToBinaryString(this byte[] bytes, string sep = "") => string.Join(sep, bytes.Select(b => b.ToBinaryString()));

    public static string ToBase64String(this byte[] bytes, string eol = "")
    {
        var s = Convert.ToBase64String(bytes);
        if (eol != "")
        {
            s = s.SplitBySize(64).JoinToString(eol);
        }
        return s;
    }

    public static bool IsPrintableAscii(this byte b, bool crlftab = false) =>
        b >= 32 && b <= 126 || crlftab && (b == 0x09 || b == 0x0A || b == 0x0B || b == 0x0D);
    public static bool IsPrintableAscii(this byte[] bytes, bool crlftab = false) => bytes.All(b => b.IsPrintableAscii(crlftab));
    /// <summary>
    /// 注意：并非所有Byte都是可读字符。
    /// </summary>
    public static string ToAsciiString(this byte[] bytes) => Encoding.ASCII.GetString(bytes);
    public static string ToUtf8String(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

    #endregion


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

    public static bool StartsWith(this byte[] array, byte[] data)
    {
        if (data.Length == 0) throw new ArgumentException($"Data length is 0.");
        if (array.Length < data.Length) return false;
        return array.SubFirst(data.Length).SequenceEqual(data);
    }

    public static byte[] DeepCopy(this byte[] bytes)
    {
        var copy = new byte[bytes.Length];
        Buffer.BlockCopy(bytes, 0, copy, 0, bytes.Length);
        return copy;
    }
}
