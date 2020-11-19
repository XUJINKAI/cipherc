using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.Cli;
using CipherTool.Exceptions;
using CipherTool.Interpret;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class GrammarParserTest : TestBase
    {
        public GrammarParserTest(ITestOutputHelper output) : base(output)
        {
        }

        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase("help"),
            new TestCase("vars", (lines, error) => Assert.Empty(lines)),
            new TestCase("vars", "a = 010203FF") { PreAction = ctx => ctx.Variables.Add("a", Data.FromHexString("010203FF")) },
            new TestCase("vars", "a = ABCDEF", "b = 123456", "a234567890 = 123456ABCDEF") { PreAction = ctx =>
            {
                ctx.Variables.Add("a", Data.FromHexString("ABCDEF"));
                ctx.Variables.Add("b", Data.FromHexString("123456"));
                ctx.Variables.Add("a234567890", Data.FromHexString("123456ABCDEF"));
            }},
            new TestCase("pipe", EmptyAssert){ PreAction = _ => ConsoleHelper.MockConsoleInput("1234567890") },
        };

        private static IEnumerable<object[]> GeTestCases()
        {
            foreach (var c in Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(GeTestCases))]
        public void Test(params TestCase[] array) => array.ForEach(c => TestCase(c));
    }
}
