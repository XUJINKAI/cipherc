using System.Collections.Generic;
using System.IO;
using System.Linq;
using CipherTool.Cli;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class DataSourceTest : TestBase
    {
        public DataSourceTest(ITestOutputHelper output) : base(output)
        {
        }

        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase("hex ABCD print hex", "ABCD"),
            new TestCase("bin 1001 print bin", "00001001"),
            new TestCase("base64 //// print base64", "////"),
            new TestCase("url %AB%CD print hex", "ABCD"),
            new TestCase("txt 1234 print txt", "1234"),
            new TestCase("var x print txt", "1234") { PreAction = ctx => ctx.Variables.Add("x", "1234") },
            new TestCase("rand 10 print hex", (lines,error) => Assert.True(lines.First().Length == 20)),
            new TestCase("file tmp print hex", "1234") { PreAction = _ => File.WriteAllBytes("tmp", Data.FromHexString("1234")) },
            new TestCase("pipe txt print txt", "1234567890") { PreAction = _ => ConsoleHelper.MockConsoleInput("1234567890") },
            new TestCase("pipe file print hex", "1234") { PreAction = _ => {
                File.WriteAllBytes("tmp", Data.FromHexString("1234"));
                ConsoleHelper.MockConsoleInput("tmp");
            }},
        };

        private static IEnumerable<object[]> TestCases()
        {
            foreach (var c in Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Test(params TestCase[] array) => array.ForEach(c => TestCase(c));
    }
}
