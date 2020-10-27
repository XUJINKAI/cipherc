using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public class Grammar : TestBase
    {
        public Grammar(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("303132")]
        public void MultiPostfix(string hex)
        {
            TestOutput($"hex {hex} print hex print base64 print txt", (lines, err) =>
            {
                Assert.Equal(3, lines.Length - 1);
                Assert.Equal(hex, lines[0]);
                Assert.Equal(Data.FromHexString(hex).ToBase64String(), lines[1]);
                Assert.Equal(Data.FromHexString(hex).ToAsciiString(), lines[2]);
            });
        }
    }
}
