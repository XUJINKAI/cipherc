using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class AutoPrintTest : TestBase
    {
        public AutoPrintTest(ITestOutputHelper output) : base(output)
        {
        }

        private static TestOutputDelegate TestVeryShortData => (lines, error) =>
        {
            Assert.True(lines.All(l => l.Length < 100));
            Assert.Contains(lines, l => l.StartsWith("HEX"));
            Assert.Contains(lines, l => l.StartsWith("ASCII"));
            Assert.Contains(lines, l => l.StartsWith("BASE64"));
        };

        private static TestOutputDelegate TestVeryLongData => (lines, error) =>
        {
            Assert.True(lines.All(l => l.Length < 100));
            Assert.Contains(lines, l => l.StartsWith("LENGTH"));
            Assert.Contains(lines, l => l.StartsWith("HEX"));
            Assert.Contains(lines, l => l.StartsWith("MD5"));
            Assert.Contains(lines, l => l.StartsWith("SM3"));
        };

        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase("txt 1234", TestVeryShortData),
            new TestCase("txt "+"1234567890".Repeat(8), TestVeryLongData),
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
