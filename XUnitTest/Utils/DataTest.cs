using Xunit;
using Xunit.Abstractions;
using CipherTool;

namespace CipherTool.Test.Utils
{
    public class DataTest : TestBase
    {
        public DataTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BytesConcat()
        {
            Assert.Equal(Data.FromHexString("1234") + Data.FromHexString("ABCD"), Data.FromHexString("1234ABCD"));
        }

        [Fact]
        public void BytesRepeat()
        {
            Assert.Equal(Data.FromHexString("1234") * 2, Data.FromHexString("12341234"));
        }
    }
}