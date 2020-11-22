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
    public class CommandTest : TestBase
    {
        public CommandTest(ITestOutputHelper output) : base(output)
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
            new TestCase("vars", (lines,error) => Assert.True(lines.All(l => l.Length < 100))) { PreAction = ctx =>
            {
                ctx.Variables.Add("a", Data.FromHexString("ABCD"));
                ctx.Variables.Add("very_long", "abcdefg".Repeat(100));
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
