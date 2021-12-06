using System.Collections.Generic;
using System.Linq;

namespace cipherc
{
    public enum DumpForm
    {
        // guess dump format
        Auto,

        Hex,
        Hex_CStr,
        Base64,
        Base64_Endline,
        HexDump,

        UTF8,
        GBK,
    }

    public static class DumpFormExtensions
    {
        public static Dictionary<string, DumpForm> DumpFormKeyDict { get; } = new Dictionary<string, DumpForm>()
        {
            {"auto", DumpForm.Auto},
            {"hex", DumpForm.Hex},
            {"hex.c", DumpForm.Hex_CStr },
            {"base64", DumpForm.Base64 },
            {"base64n", DumpForm.Base64_Endline },
            {"hexdump", DumpForm.HexDump},
            {"utf8", DumpForm.UTF8 },
            {"gbk", DumpForm.GBK},
        };

        public static string GetKeyName(this DumpForm form)
        {
            return DumpFormKeyDict.First(kv => kv.Value == form).Key;
        }

        public static DumpForm? GetDumpForm(string key)
        {
            if (DumpFormKeyDict.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        public static string[] GetDumpFormKeyList()
        {
            return DumpFormKeyDict.Keys.ToArray();
        }

        public static string AutoDump(this byte[] bytes, out DumpForm form)
        {
            DumpForm? outForm = null;
            var encoder = CharsetEncoder.GuessEncoder(bytes);
            if (encoder != null)
            {
                switch (encoder.Charset.ToLower())
                {
                    case "gb18030":
                    case "gb2312":
                        outForm = DumpForm.GBK;
                        break;
                    case "utf-8":
                    case "ascii":
                        outForm = DumpForm.UTF8;
                        break;
                }
            }

            outForm ??= bytes.Length <= 32 ? DumpForm.Hex : DumpForm.HexDump;
            form = outForm.Value;
            return bytes.Dump(form);
        }

        public static string Dump(this byte[] bytes, DumpForm form, bool addAutoDumpPrefix = true)
        {
            return form switch
            {
                DumpForm.Auto => ((Func<string>)(() =>
                {
                    var s = bytes.AutoDump(out var autoFrom);
                    return (addAutoDumpPrefix ? $"[{autoFrom}] " : "") + s;
                }))(),
                DumpForm.Hex => $"{bytes.ToHexString()}",
                DumpForm.Hex_CStr => $"\\x{bytes.ToHexString("\\x")}",
                DumpForm.Base64 => $"{bytes.ToBase64String()}",
                DumpForm.Base64_Endline => $"{bytes.ToBase64String(ENV.EndOfLine)}",
                DumpForm.HexDump => bytes.ToHexDumpText(ENV.EndOfLine),
                DumpForm.UTF8 => ENV.UTF8Encoder.Decode(bytes),
                DumpForm.GBK => ENV.GBKEncoder.Decode(bytes),
                _ => throw new NotImplementedException($"Unknown BytesForm {form}."),
            };
        }
    }
}
