using System;
using System.IO;
using CipherTool.Transform;

namespace CipherTool
{
    /// <summary>
    /// 自定义数据类，可由string或bytes隐式转换，方便函数入参
    /// </summary>
    public struct Data : IEquatable<Data>, IEquatable<byte[]>
    {
        private byte[] Bytes;

        public int Length => Bytes.Length;

        public Data(byte[] byteArray)
        {
            Bytes = byteArray;
        }

        /// <summary>
        /// 使用UTF8编码字符串初始化
        /// </summary>
        /// <param name="plainString"></param>
        public Data(string plainString)
        {
            Bytes = plainString.GetBytes();
        }

        public string ToAsciiString() => Bytes.ToAsciiString();
        public string ToBase64String() => Convert.ToBase64String(Bytes);
        public string ToHexString(string sep = "") => Bytes.ToHexString(sep);
        public void SaveToFile(string path)
        {
            File.WriteAllBytes(path, Bytes);
        }

        public static Data FromBytes(byte[] bytes) => new Data(bytes);
        public static Data FromPlainString(string plainString) => new Data(plainString);
        public static Data FromBase64String(string base64) => new Data(Convert.FromBase64String(base64));
        public static Data FromHexString(string hexString) => new Data(FromHexStringToByteArray(hexString));
        public static Data FromFile(string path) => new Data(File.ReadAllBytes(path));

        public static implicit operator byte[](Data data) => data.Bytes;
        public static implicit operator Data(byte[] byteArray) => new Data(byteArray);
        public static implicit operator Data(string utf8String) => new Data(utf8String);

        public static bool operator ==(Data data1, Data data2) => data1.Equals(data2);
        public static bool operator !=(Data data1, Data data2) => !data1.Equals(data2);
        public static bool operator ==(Data data, string str) => data.Equals(str);
        public static bool operator !=(Data data, string str) => !data.Equals(str);
        public static bool operator ==(Data data, byte[] bytes) => data.Equals(bytes);
        public static bool operator !=(Data data, byte[] bytes) => !data.Equals(bytes);


        /// <summary>
        /// Encoding to Hex String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToHexString("");

        public bool Equals(Data other) => this.Bytes.SequanceEqual(other.Bytes);

        public bool Equals(byte[]? other) => this.Bytes.SequanceEqual(other);

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Data data => this.Bytes.SequanceEqual(data.Bytes),
                string str => this.Bytes.SequanceEqual(str.GetBytes()),
                byte[] bytes => this.Bytes.SequanceEqual(bytes),
                _ => false,
            };
        }

        public override int GetHashCode()
        {
            return this.Bytes.GetHashCode();
        }

        /// <summary>
        /// 调用字符串为16进制编码字串，将此字串还原为bytes。
        /// 此函数为byte[].ToHexString()的反函数。
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] FromHexStringToByteArray(string hexStr)
        {
            if (hexStr == null) throw new ArgumentNullException(nameof(hexStr));
            if (hexStr.Length % 2 != 0) throw new ArgumentException($"HexString's length must be even. Value: [{hexStr.Length}] {hexStr}.");

            int NumberChars = hexStr.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexStr.Substring(i, 2), 16);
            return bytes;
        }

    }
}
