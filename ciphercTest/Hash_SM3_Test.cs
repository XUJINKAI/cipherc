using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciphercTest
{
    public class Hash_SM3_Test : TestBase
    {
        public Hash_SM3_Test(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void SM3_0000_Bytes()
        {
            var result = RunCommand($"sm3 0000").GetByteResult();
            var expect = HEX("AF83A966222057AC761246A7543C580D9111014F4E5E3CB1281DB33151160335");
            Assert.Equal(expect, result);
        }


        [Fact]
        public void Hash_Text()
        {
            var result = RunCommand($"sm3 abc --inform utf8");
            var expect = "66C7F0F462EEEDD9D1F2D46BDC10E4E24167C4875CF2F7A2297DA02B8F4BA8E0";
            Assert.Equal(expect + EOL, result.GetOutResult());
            Assert.Equal(HEX(expect), result.GetByteResult());
        }


        [Theory]
        [InlineData("sm3", "AF83A966222057AC761246A7543C580D9111014F4E5E3CB1281DB33151160335")]
        [InlineData("md5", "C4103F122D27677C9DB144CAE1394A66")]
        [InlineData("sha1", "1489F923C4DCA729178B3E3233458550D8DDDF29")]
        [InlineData("sha256", "96A296D224F285C67BEE93C30F8A309157F0DAA35DC5B87E410B78630A09CFC7")]
        //[InlineData("sha384", "1DD6F7B457AD880D840D41C961283BAB688E94E4B59359EA45686581E90FECCEA3C624B1226113F824F315EB60AE0A7C")]
        [InlineData("sha512", "5EA71DC6D0B4F57BF39AADD07C208C35F06CD2BAC5FDE210397F70DE11D439C62EC1CDF3183758865FD387FCEA0BADA2F6C37A4A17851DD1D78FEFE6F204EE54")]
        public void Hash_0000(string hash, string output_hex)
        {
            var result = RunCommand($"{hash} --inform hex --dump hex 0000");
            Assert.Equal(output_hex + EOL, result.GetOutResult());
            Assert.Equal(HEX(output_hex), result.GetByteResult());
        }


        [Fact]
        public void Exp_ArgNull()
        {
            Assert.Throws<ParseErrorException>(() =>
            {
                RunCommand("sm3");
            });
        }


        [Fact]
        public void Exp_ArgError()
        {
            Assert.Throws<ArgumentErrorException>(() =>
            {
                RunCommand("sm3 xxxx");
            });
        }
    }
}
