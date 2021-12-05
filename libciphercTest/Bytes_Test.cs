using libcipherc.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libciphercTest
{
    public class Bytes_Test : TestBase
    {
        public Bytes_Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HexDump_Rand()
        {
            OutputLine(GetRandom(130).ToHexDumpText());
        }
    }
}
