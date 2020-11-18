using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class UnexpectedSituationTest : TestBase
    {
        public UnexpectedSituationTest(ITestOutputHelper output) : base(output)
        {
        }


        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase($"hex 31323301FF print txt",  "123�"),
        };

        private static IEnumerable<object[]> GeUrlTestCases()
        {
            foreach (var c in Datas)
                yield return new object[] { c };
        }

        [Theory]
        [MemberData(nameof(GeUrlTestCases))]
        public void Test(params TestCase[] array) => array.ForEach(c => TestCase(c));
    }
}
