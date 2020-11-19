using System.Collections.Generic;
using System.Linq;
using CipherTool.Cli;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class Examples : TestBase
    {
        public Examples(ITestOutputHelper output) : base(output)
        {
        }

        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase("hex 31323334353637383930 print txt then print hex base64 MTIzNDU2Nzg5MA==", "1234567890", "31323334353637383930"),
            new TestCase("file cipherc.dll", EmptyAssert),
            new TestCase("var x is txt 1234567890 then var x print hex", "31323334353637383930"),
            new TestCase("rand 64 print hex", (lines,error) => Assert.True(lines[0].Length == 64 * 2)),
            new TestCase("rand 64 print bin", (lines,error) => Assert.True(lines[0].Length == 64 * 8)),
        };

        [Fact]
        public void Test() => Datas.ForEach(c => TestCase(c));
    }
}
