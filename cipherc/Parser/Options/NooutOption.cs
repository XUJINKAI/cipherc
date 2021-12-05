using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Parser.Options
{
    public class NooutOption : Option<bool>
    {
        public const string NAME = "--noout";

        public NooutOption() : base(NAME)
        {
            Description = "";
        }
    }
}
