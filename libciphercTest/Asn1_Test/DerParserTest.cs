using libcipherc;
using libcipherc.ASN1;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace libciphercTest
{
    public class DerParserTest : TestBase
    {
        public DerParserTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void DumpX509()
        {
            var content = LoadTestFile("cert/x509.pem");
            var bytes = ASN1Encoder.TryParse(content);
            var dump = new DerParser().Dump(bytes);
            OutputLine(dump);
        }
    }
}
