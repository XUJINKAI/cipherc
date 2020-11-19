using System.Collections.Generic;
using CipherTool.Cli;
using CipherTool.Exceptions;
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
            // Decode error
            new TestCase($"bin 1234", typeof(DecodeException)),
            new TestCase($"base64 ----", typeof(DecodeException)),
            new TestCase($"txt ---- decode base64", typeof(DecodeException)),
            // parser error
            new TestCase("hex", typeof(ExpectedMoreTokenException), ExpectedMoreTokenException.KeyFragment),
            new TestCase("print hex txt", typeof(ExpectedMoreTokenException), ExpectedMoreTokenException.KeyFragment),
            new TestCase("unknown token", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("hex 1234 repeat x", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("rand x", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("print hex pipe", typeof(NoPipeInputException), NoPipeInputException.KeyFragment) { PreAction = _ => ConsoleHelper.MockConsoleInput(null) },
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
