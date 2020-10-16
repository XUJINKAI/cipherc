using System;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public class DataExpression_Test : TestBase
    {
        public DataExpression_Test(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("hex", "plain", "31323301FF", "123\x01?")]
        [InlineData("plain", "hex", "1234567890", "31323334353637383930")]
        [InlineData("plain", "base64", "1234567890", "MTIzNDU2Nzg5MA==")]
        public void TransFormat(string inputFormat, string outputFormat,
            string input, string assertOutput)
        {
            TestOutput($"from {inputFormat} data {input} to {outputFormat} data", (o, e) =>
            {
                Assert.Equal(assertOutput + ENDLINE, o);
            });
        }
    }
}
