using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace cipherc.Parser
{
    public class Runner
    {
        public System.CommandLine.Parsing.Parser Parser { get; set; }

        public Runner()
        {
            Parser = ParserGenerator.CreateDefaultBuilder().Build();
        }

        public int RunCommand(string[] args, IOutput? output = null)
        {
            output ??= new DefaultOutput();
            try
            {
                var result = Parser.Parse(args);
                return result.Invoke(output);
            }
            catch (AbstractRunnerException ex)
            {
                output.ErrorLine(ex.Message);
                if (ex is ParseErrorException pe && pe.CommandHelp != null)
                {
                    output.WriteLine("");
                    output.WriteLine(pe.CommandHelp);
                }
                return -1;
            }
            catch
            {
                throw;
            }
        }

        public void IntoShell()
        {
            if (ENV.DEBUG) Console.WriteLine(ENV.DumpEnv());
            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = new AutoCompletionHandler();
            var console = new DefaultOutput();
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("cipherc>");
                Console.ResetColor();
                var line = ReadLine.Read();

                if (line == null) continue;
                var lineArgs = CommandLineHelper.CommandLineToArgs(line);
                if (lineArgs.Length == 0)
                {
                    continue;
                }

                RunCommand(lineArgs, console);
            } while (true);
        }
    }
}
