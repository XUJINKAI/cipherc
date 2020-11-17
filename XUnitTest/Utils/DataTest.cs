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
        public void StringConvert()
        {
            Assert.Equal("00000001 00001010 01100100 11111111 11001000 00010100 00000010", new byte[] { 1, 10, 100, 255, 200, 20, 2 }.ToBinaryString(" "));
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

            Assert.False(true);
        }
    }
}