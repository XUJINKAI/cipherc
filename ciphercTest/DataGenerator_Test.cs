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
            Assert.Equal(HEX("00"), RunCommand("zero 1 --verbose 3").GetByteResult());
        }


        [Fact]
        public void Zero_8()
        {
            Assert.Equal(HEX("0000000000000000"), RunCommand("zero 8").GetByteResult());
        }


        [Fact]
        public void Zero_8_base64()
        {
            var expect = Convert.ToBase64String(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.Equal(expect.GetBytes(), RunCommand("zero 8 --outform base64").GetByteResult());
        }

        [Fact]
        public void Rand_32()
        {
            var result = RunCommand("rand 32");
            Assert.Equal(32, result.GetByteResult().Length);
        }
    }
}
