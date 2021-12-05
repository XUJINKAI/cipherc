using cipherc.Handler;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Parser.Options
{
    public class InformOption : Option<string>
    {
        public const string NAME = "--inform";

        public InformOption(string defaultValue) : base(NAME, () => defaultValue)
        {
            ArgumentHelpName = "InFormat";
            Description = @$"Input format
Allow Value: {InputDataModel.ValidInforms}";
        }
    }
}
