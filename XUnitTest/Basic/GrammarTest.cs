using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Basic
{
    public class GrammarTest : TestBase
    {
        public GrammarTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Sentences()
        {
            var data1 = GetRandom(10);
            var data2 = GetRandom(10);
            TestOutput($"hex {data1.ToHexString()} print ascii then print hex base64 {data2.ToBase64String()}", (output, error) =>
            {
                Assert.Equal(data1.ToAsciiString(), output[0]);
                Assert.Equal(data2.ToHexString(), output[1]);
            });
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
