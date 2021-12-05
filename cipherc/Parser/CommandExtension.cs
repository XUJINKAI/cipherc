using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace cipherc.Parser
{
    public static class CommandExtension
    {
        public static RootCommand WithHandler(this RootCommand command, Delegate @delegate)
        {
            command.Handler = CommandHandler.Create(@delegate);
            return command;
        }

        public static RootCommand AddGlobalOptions(this RootCommand command, params Option[] options)
        {
            foreach (var option in options)
            {
                command.AddGlobalOption(option);
            }
            return command;
        }

        public static Command WithHandler(this Command command, Delegate @delegate)
        {
            command.Handler = CommandHandler.Create(@delegate);
            return command;
        }

        public static Command AddOptions(this Command command, params Option[] options)
        {
            foreach (var option in options)
            {
                command.AddOption(option);
            }
            return command;
        }

        public static Command WithAlias(this Command command, params string[] names)
        {
            foreach (var name in names)
            {
                command.AddAlias(name);
            }
            return command;
        }
    }
}
