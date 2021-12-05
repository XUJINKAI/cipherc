using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciphercTest
{
    public class Help_Test : TestBase
    {
        public Help_Test(ITestOutputHelper output) : base(output)
        {
        }

        [Fact] public void Main() => RunCommand("--help");

        [Fact] public void Env() => RunCommand("env");

        [Fact] public void SM3() => RunCommand("sm3 --help");

        [Fact] public void Zero() => RunCommand("zero -h");

        [Fact] public void Rand() => RunCommand("rand --help");
    }
}
