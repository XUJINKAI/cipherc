using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Parser.Options
{
    public class OutOption : Option<string?>
    {
        public const string NAME = "--out";

        public OutOption() : base(NAME)
        {
            ArgumentHelpName = "FilePath";
            Description = "Output file path";
        }
    }
}
