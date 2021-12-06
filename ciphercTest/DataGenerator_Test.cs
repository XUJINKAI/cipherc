using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ciphercTest
{
    public class DataGenerator_Test : TestBase
    {
        public DataGenerator_Test(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Zero_1_Verbose()
        {
            var result = RunCommand("zero 1 --verbose");
            Assert.Equal(HEX("00"), result.GetByteResult());
        }


        [Fact]
        public void Zero_8()
        {
            var result = RunCommand("zero 8");
            Assert.Equal("0000000000000000" + EOL, result.GetOutResult());
            Assert.Equal(HEX("0000000000000000"), result.GetByteResult());
        }


        [Fact]
        public void Zero_8_base64()
        {
            var result = RunCommand("zero 8 --dump base64");
            var data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            var data_base64 = Convert.ToBase64String(data);
            Assert.Equal(data_base64 + EOL, result.GetOutResult());
            Assert.Equal(data, result.GetByteResult());
        }

        [Fact]
        public void Rand_32()
        {
            var result = RunCommand("rand 32");
            Assert.Equal(32, result.GetByteResult().Length);
        }
    }
}
