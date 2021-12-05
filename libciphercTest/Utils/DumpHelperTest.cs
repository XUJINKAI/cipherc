using libcipherc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace libciphercTest.Utils
{
    public class DumpHelperTest : TestBase
    {
        public DumpHelperTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Auto2Columns()
        {
            var dumper = new DumpHelper(0, 0);

            dumper.SetLineContent(0, "abc");
            dumper.SetLineContent(1, "abcdef");
            dumper.SubmitLine();

            dumper.AppendLine("abcdef", "abc");

            OutputLine(dumper.ToString());
        }
    }
}
