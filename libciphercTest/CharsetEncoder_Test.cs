using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace libciphercTest
{
    public class CharsetEncoder_Test : TestBase
    {
        public CharsetEncoder_Test(ITestOutputHelper output) : base(output)
        {
        }


        static CharsetEncoder_Test()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }


        [Fact]
        public void ListCharsets()
        {
            var list = Encoding.GetEncodings().Select(e => e.DisplayName).ToList();
            list.Sort();
            OutputLine(list.JoinToString("\n"));
        }


        [Fact]
        public void ListCodePage()
        {
            var output = (object codeOrName) =>
            {
                try
                {
                    Encoding encoding = codeOrName switch
                    {
                        int codepage => Encoding.GetEncoding(codepage),
                        string codename => Encoding.GetEncoding(codename),
                        _ => throw new NotImplementedException(),
                    };
                    OutputLine($"{codeOrName}: {encoding.EncodingName}, {encoding.CodePage}");
                }
                catch
                {
                    OutputLine($"{codeOrName}: Not support");
                }
            };
            output("UTF8");
            output("UTF-8");
            output("GB2312");
            output("GBK");
            output("GB18030");
            output(936);
            output(54936);
        }


        [Fact]
        public void ChineseEncode()
        {
            string x = "OneQuick - 热键工具: Windows热键辅助工具，简化操作，提高效率。";
            var bytes936 = CharsetEncoder.GetEncoder(936).Encode(x);
            var bytes54936 = CharsetEncoder.GetEncoder(54936).Encode(x);
            OutputLine(bytes936.ToHexString());
            OutputLine(bytes54936.ToHexString());
            var (charset936, _) = CharsetEncoder.GuessCharset(bytes936, 0.0f);
            OutputLine($"936 Result: {charset936 ?? "null"}");
            var (charset54936, _) = CharsetEncoder.GuessCharset(bytes54936, 0.0f);
            OutputLine($"54936 Result: {charset54936 ?? "null"}");
        }


        [Theory]
        [InlineData("00000000", null, false)]
        [InlineData("313233616263", "ASCII", false)]
        [InlineData("3132336162630A0D", "ASCII", false)]
        [InlineData("4F6E65517569636B202D20C8C8BCFCB9A4BEDF3A2057696E646F7773C8C8BCFCB8A8D6FAB9A4BEDFA3ACBCF2BBAFB2D9D7F7A3ACCCE1B8DFD0A7C2CAA1A3", "gb18030", false)]
        public void BytesGuessCharset(string hex, string? expCharset, bool expHasBOM)
        {
            var bytes = HEX(hex);
            var (charset, hasBOM) = CharsetEncoder.GuessCharset(bytes, 0.0f);
            OutputLine($"Hex: {hex}");
            OutputLine($"Result: {charset ?? "null"}, {hasBOM}");
            Assert.Equal(expCharset, charset);
            Assert.Equal(expHasBOM, hasBOM);
        }


        [Fact]
        public void EncodingBOM()
        {
            var bytes = HEX("EFBBBF41");
            var s = "A";
            var encoded = CharsetEncoder.GetUTF8Encoder(true).Encode(s);
            Assert.Equal(bytes, encoded);
        }

        [Fact]
        public void DecodingWithBOM()
        {
            var bytes = HEX("EFBBBF41");
            var decodeStr = CharsetEncoder.GuessString(bytes);
            Assert.Equal("A", decodeStr);
        }
    }
}
