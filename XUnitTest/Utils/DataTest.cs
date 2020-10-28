using Xunit;
using Xunit.Abstractions;
using CipherTool;
using System.Linq;

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
            var left = "1234";
            var right = "ABCD";
            var left_data = Data.FromHexString(left);
            var right_data = Data.FromHexString(right);
            var left_bytes = Data.FromHexString(left).GetBytes();
            var right_bytes = Data.FromHexString(right).GetBytes();
            var result = Data.FromHexString(left + right);

            Assert.Equal(left_data + right_data, result);
            Assert.Equal(left_data + right_bytes, result);
            Assert.Equal(left_bytes + right_data, result);
        }

        [Fact]
        public void BytesRepeat()
        {
            var hex = "1234";
            var times = 3;
            var hex_times = hex.Repeat(times);
            var data = Data.FromHexString(hex);

            Assert.Equal(data * times, Data.FromHexString(hex_times));

            AppendLine(hex_times);
        }
    }
}