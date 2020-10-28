using System;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public class DataExpression : TestBase
    {
        public DataExpression(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(16)]
        [InlineData(64)]
        public void RandomBytes(int bytes)
        {
            TestOutput($"rand {bytes} print hex", (lines, err) =>
            {
                Assert.True(lines[0].Length == bytes * 2);
            });
        }

        [Theory]
        [InlineData("hex", "31323301FF", "31323301FF")]
        [InlineData("txt", "1234567890", "31323334353637383930")]
        [InlineData("base64", "MTIzNDU2Nzg5MA==", "31323334353637383930")]
        public void LoadData(string inputFormat, string input, string assertOutput)
        {
            TestOutput($"{inputFormat} {input} print hex", (lines, err) =>
            {
                Assert.Equal(assertOutput, lines[0]);
            });
        }

        [Theory]
        [InlineData("hex", "txt", "31323301FF", "123\x01?")]
        [InlineData("txt", "hex", "1234567890", "31323334353637383930")]
        [InlineData("txt", "base64", "1234567890", "MTIzNDU2Nzg5MA==")]
        public void TransFormat(string inputFormat, string outputFormat,
            string input, string assertOutput)
        {
            TestOutput($"{inputFormat} {input} print {outputFormat}", (lines, err) =>
            {
                Assert.Equal(assertOutput, lines[0]);
            });
        }
    }
}
