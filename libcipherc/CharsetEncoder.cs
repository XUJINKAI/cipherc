using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc
{
    /// <summary>
    /// 对Encoding, Ude.CharsetDetector的封装
    /// </summary>
    public class CharsetEncoder
    {
        public string Charset { get; }
        public bool UseBOM { get; }
        public Encoding Encoding { get; }

        public CharsetEncoder(string charset, bool useBom)
        {
            Charset = charset;
            UseBOM = useBom;
            Encoding = charset switch
            {
                "ASCII" => new ASCIIEncoding(),
                "UTF-8" => new UTF8Encoding(useBom),
                "UTF-16BE" => new UnicodeEncoding(true, useBom),
                "UTF-16LE" => new UnicodeEncoding(false, useBom),
                "UTF-32BE" => new UTF32Encoding(true, useBom),
                "UTF-32LE" => new UTF32Encoding(false, useBom),
                _ => Encoding.GetEncoding(charset),
            };
        }

        public CharsetEncoder(Encoding encoding)
        {
            Charset = encoding.EncodingName;
            UseBOM = !encoding.Preamble.IsEmpty;
            Encoding = encoding;
        }

        public CharsetEncoder(int codepage) : this(Encoding.GetEncoding(codepage)) { }

        public byte[] Encode(string s, bool? forceBOM = null)
        {
            if (forceBOM ?? UseBOM)
            {
                return GetBOM(Charset).Concat(Encoding.GetBytes(s));
            }
            else
            {
                return Encoding.GetBytes(s);
            }
        }

        public string Decode(byte[] data)
        {
            var bom = GetBOM(Charset);
            if (bom.Length > 0 && data.StartsWith(bom))
            {
                return Encoding.GetString(data.SubArray(bom.Length));
            }
            else
            {
                return Encoding.GetString(data);
            }
        }

        public override string ToString()
        {
            return $"{Encoding.EncodingName}{(UseBOM ? " With BOM" : "")}";
        }


        public static byte[] GetBOM(string charset)
        {
            return charset switch
            {
                "UTF-8" => new byte[] { 0xEF, 0xBB, 0xBF },
                "UTF-16BE" => new byte[] { 0xFE, 0xFF },
                "UTF-16LE" => new byte[] { 0xFF, 0xFE },
                "UTF-32BE" => new byte[] { 0x00, 0x00, 0xFE, 0xFF },
                "UTF-32LE" => new byte[] { 0xFF, 0xFE, 0x00, 0x00 },
                _ => Array.Empty<byte>(),
            };
        }

        public static CharsetEncoder GetEncoder(string charset, bool useBOM = false) => new CharsetEncoder(charset, useBOM);
        public static CharsetEncoder GetEncoder(int codepage) => new CharsetEncoder(codepage);
        public static CharsetEncoder GetUTF8Encoder(bool bom = false) => new CharsetEncoder("UTF-8", bom);
        public static CharsetEncoder GetGB18030Encoder() => new CharsetEncoder("GB18030", false);


        public static (string?, bool) GuessCharset(byte[] data, float threshold = 0.5f)
        {
            using var stream = new MemoryStream(data);

            Ude.CharsetDetector detector = new();
            detector.Reset();
            detector.Feed(stream);
            detector.DataEnd();

            string? charset = null;
            bool hasBOM = false;

            if (detector.Confidence >= threshold)
            {
                var bom = GetBOM(detector.Charset);
                hasBOM = bom.Length > 0 && data.StartsWith(bom);
                charset = detector.Charset;
                switch (charset)
                {
                    case "ASCII":
                        if (!data.IsPrintableAscii(true))
                        {
                            charset = null;
                        }
                        break;
                }
            }
            return (charset, hasBOM);
        }

        public static CharsetEncoder? GuessEncoder(byte[] data, float threshold = 0.5f)
        {
            var (charset, hasBOM) = GuessCharset(data, threshold);
            if (charset != null)
            {
                return new CharsetEncoder(charset, hasBOM);
            }
            return null;
        }

        public static string? GuessString(byte[] data, float threshold = 0.5f)
        {
            var encoder = GuessEncoder(data, threshold);
            if (encoder == null)
            {
                return null;
            }
            return encoder.Decode(data);
        }


        public static void RegisterEncodings()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        static CharsetEncoder()
        {
            RegisterEncodings();
        }

    }
}
