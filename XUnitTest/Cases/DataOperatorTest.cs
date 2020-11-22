using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class DataOperatorTest : TestBase
    {
        public DataOperatorTest(ITestOutputHelper output) : base(output)
        {
        }

        // concat

        private static List<TestCase> Concat_Datas => new List<TestCase>()
        {
            new TestCase("var x is hex ABCD concat hex 1234 then print hex var x", "ABCD1234"),
        };

        private static IEnumerable<object[]> Concat_TestCases()
        {
            foreach (var c in Concat_Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(Concat_TestCases))]
        public void ConcatTest(params TestCase[] array) => array.ForEach(c => TestCase(c));

        // repeat

        private static List<TestCase> Repeat_Datas => new List<TestCase>()
        {
            new TestCase("hex 1234 repeat 4 print hex", "1234123412341234"),
        };

        private static IEnumerable<object[]> Repeat_TestCases()
        {
            foreach (var c in Repeat_Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(Repeat_TestCases))]
        public void RepeatTest(params TestCase[] array) => array.ForEach(c => TestCase(c));

        // sub

        private static List<TestCase> Sub_Datas => new List<TestCase>()
        {
            new TestCase("hex ABCD1234FF sub 2 2 print hex", "1234"),
        };

        private static IEnumerable<object[]> Sub_TestCases()
        {
            foreach (var c in Sub_Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(Sub_TestCases))]
        public void SubTest(params TestCase[] array) => array.ForEach(c => TestCase(c));
    }
}
