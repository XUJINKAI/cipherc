using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Parser.Options
{
    public class DumpOption : Option<string>
    {
        public const string NAME = "--dump";

        public DumpOption() : base(NAME)
        {
            ArgumentHelpName = "DumpFormat";
            Description = @"Dump Forms, separate with comma, e.g. ""hex,base64""
Allow value: auto, hex, hex.c, hexdump, base64, base64n";
        }

        public static IReadOnlyList<DumpForm?> ParseValue(string value, ICommand command)
        {
            var forms = value.Split(",");
            return forms.Select<string, DumpForm?>(form =>
            {
                return form.ToLower() switch
                {
                    "auto" => null,
                    "hex" => DumpForm.Hex,
                    "hex.c" => DumpForm.Hex_CStr,
                    "hexdump" => DumpForm.HexDump,
                    "base64" => DumpForm.Base64,
                    "base64n" => DumpForm.Base64_Endline,
                    _ => throw new ParseErrorException($"Unknown dump form: {form}", command),
                };
            }).ToList();
        }

        private static CharsetEncoder ParseCharsetEncoder(string encode)
        {
            var x = @"BytesForm[,Charset[,EOL]] output format, e.g. hex; hexdump,utf8; base64,ascii,lf
BytesForm: auto, hex, hex.c, hexdump, base64, base64n
Charset: utf8[bom], utf16be[bom], utf16le[bom], utf32be[bom], utf32le[bom], ascii, gbk
EOL: lf, cr, crlf";
            string encodeStr = encode.ToLower();
            bool useBOM = encodeStr.EndsWith("bom");
            string charset = (useBOM ? encodeStr.TrimEnd("bom") : encodeStr) switch
            {
                "ascii" => "ASCII",
                "utf8" => "UTF-8",
                "utf16be" => "UTF16-BE",
                "utf16le" => "UTF16-LE",
                "utf32be" => "UTF32-BE",
                "utf32le" => "UTF32-LE",
                "gbk" => "GBK",
                _ => throw new ParseErrorException($"Unknown Charset {encode}"),
            };
            //DefaultOutput.LogLine(LogLevel.Verbose, $"Charset: {charset}, BOM: {useBOM}");
            return new CharsetEncoder(charset, useBOM);
        }

        private static string ParseEndOfLine(string eol)
        {
            return eol.ToLower() switch
            {
                "lf" => "\n",
                "cr" => "\r",
                "crlf" => "\r\n",
                _ => throw new ParseErrorException($"Unknown EndOfLine {eol}"),
            };
        }
    }
}
