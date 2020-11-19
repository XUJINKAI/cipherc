using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class EncodeDecodeTest : TestBase
    {
        public EncodeDecodeTest(ITestOutputHelper output) : base(output)
        {
        }

        // base64

        private static List<(string txt, string base64)> Txt_Base64_Datas => new List<(string, string)>()
        {
            ("1234567890", "MTIzNDU2Nzg5MA=="),
        };

        private static List<(string txt, string base64)> Hex_Base64_Datas => new List<(string, string)>()
        {
            ("00", "AA=="),
            ("0000", "AAA="),
            ("000000", "AAAA"),
            ("FF", "/w=="),
            ("FFFF", "//8="),
            ("FFFFFF", "////"),
            ("FFEF", "/+8="),
        };

        private static IEnumerable<object[]> GetBase64TestCases()
        {
            foreach (var (txt, base64) in Txt_Base64_Datas)
                yield return new object[] {
                    new TestCase($"txt {txt} print base64", base64),
                    new TestCase($"txt {txt} encode base64", base64),
                    new TestCase($"txt {txt} encode base64 print txt", base64),

                    new TestCase($"base64 {base64} print txt", txt),
                    new TestCase($"txt {base64} decode base64 print txt", txt),
                };
            foreach (var (hex, base64) in Hex_Base64_Datas)
                yield return new object[] {
                    new TestCase($"hex {hex} print base64", base64),
                    new TestCase($"hex {hex} encode base64", base64),
                    new TestCase($"hex {hex} encode base64 print txt", base64),

                    new TestCase($"base64 {base64} print hex", hex),
                    new TestCase($"txt {base64} decode base64 print hex", hex),
                };
        }

        [Theory]
        [MemberData(nameof(GetBase64TestCases))]
        public void Base64Test(params TestCase[] array) => array.ForEach(c => TestCase(c));

        // hex/bin

        private static List<(string hex, string bin)> Hex_Bin_Datas => new List<(string, string)>()
        {
            ("00", "00000000"),
            ("FF", "11111111"),
            ("4A", "01001010"),
            ("E0", "11100000"),
            ("0141E1B8CAFFD199", "0000000101000001111000011011100011001010111111111101000110011001"),
        };

        private static IEnumerable<object[]> GetHexBinTestCases()
        {
            foreach (var (hex, bin) in Hex_Bin_Datas)
                yield return new object[] {
                    new TestCase($"hex {hex} print bin", bin),
                    new TestCase($"hex {hex} encode bin", bin),
                    new TestCase($"hex {hex} encode bin print txt", bin),

                    new TestCase($"bin {bin} print hex", hex),
                    new TestCase($"txt {bin} decode bin print hex", hex),
                };
        }

        [Theory]
        [MemberData(nameof(GetHexBinTestCases))]
        public void HexBinTest(params TestCase[] array) => array.ForEach(c => TestCase(c));

        // url

        private static List<(string str, string url)> Url_Datas => new List<(string, string)>()
        {
            ("ABCD1234abcd-", "ABCD1234abcd-"),
            ("ABCD!@#$%^&*()-=_+ abcd", "ABCD!%40%23%24%25%5E%26*()-%3D_%2B+abcd"),
            ("ABCD中文测试 空格  abcd", "ABCD%E4%B8%AD%E6%96%87%E6%B5%8B%E8%AF%95+%E7%A9%BA%E6%A0%BC++abcd"),
        };

        private static IEnumerable<object[]> GeUrlTestCases()
        {
            foreach (var (str, url) in Url_Datas)
                yield return new object[] {
                    new TestCase($"txt \"{str}\" encode url", url),
                    new TestCase($"txt \"{str}\" encode url print txt", url),

                    new TestCase($"txt \"{url}\" decode url print txt", str),
                };
        }

        [Theory]
        [MemberData(nameof(GeUrlTestCases))]
        public void UrlTest(params TestCase[] array) => array.ForEach(c => TestCase(c));
    }
}
