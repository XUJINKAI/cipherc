using System;
using CipherTool.Interpret;

namespace CipherTool.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (ENV.DEBUG)
            {
                Console.WriteLine(Environment.CommandLine);
            }
            var ctx = new Context()
            {

            };
            var interpreter = new Interpreter(ctx);
            if (args.Length == 0)
            {
                IntoShell(interpreter);
            }
            else if (args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    case "shell":
                        IntoShell(interpreter);
                        break;
                    case "-h":
                    case "-v":
                    case "--version":
                        Console.WriteLine(HelpText.GetCliHelpText());
                        break;
                    case "--help":
                        Console.WriteLine(HelpText.GetFullHelpText());
                        break;
                    default:
                        RunCmdArgs(interpreter, args);
                        break;
                }
            }
            else
            {
                RunCmdArgs(interpreter, args);
            }
        }

        private static void IntoShell(Interpreter interpreter)
        {
            Console.WriteLine(HelpText.GetShellWhecomeText());
            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = new AutoCompletionHandler();
            do
            {
                var line = ReadLine.Read("> ");
                if (line == null) continue;

                if (line.StartsWith("!"))
                {
                    Utils.CommandRunner.System(line[1..]);
                    continue;
                }

                var lineArgs = NativeMethods.CommandLineToArgs(line);
                if (lineArgs.Length == 0) { continue; }
                if (lineArgs.Length == 1 && lineArgs[0].ToLower() == "help")
                {
                    Console.WriteLine(HelpText.GetFullHelpText());
                    continue;
                }
                RunCmdArgs(interpreter, lineArgs);
                Console.WriteLine();
            } while (true);
        }

        private static void RunCmdArgs(Interpreter interpreter, string[] args)
        {
#if DEBUG
            interpreter.Interpret(args);
#else
            try
            {
                interpreter.Interpret(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#endif
        }
    }
}
