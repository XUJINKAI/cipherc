using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Cases
{
    public class HashTest : TestBase
    {
        public HashTest(ITestOutputHelper output) : base(output)
        {
        }

        private static List<TestCase> Datas => new List<TestCase>()
        {
            new TestCase("txt 1234567890 md5", "E807F1FCF82D132F9BB018CA6738A19F"),
            new TestCase("txt 1234567890 sha1", "01B307ACBA4F54F55AAFC33BB06BBBF6CA803E9A"),
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
