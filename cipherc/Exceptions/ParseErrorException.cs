using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Exceptions
{
    public class ParseErrorException : AbstractRunnerException
    {
        public string? CommandHelp { get; set; }

        public ParseErrorException(string message) : base(message)
        {
        }

        public ParseErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private void SetCommand(ICommand command)
        {
            var output = new MemoryOutput();
            var builder = new HelpBuilder(output);
            builder.Write(command);
            CommandHelp = output.GetOutResult();
        }

        public ParseErrorException(string message, ICommand command) : base(message)
        {
            SetCommand(command);
        }

        public ParseErrorException(string message, Exception innerException, ICommand command)
            : base(message, innerException)
        {
            SetCommand(command);
        }
    }
}
