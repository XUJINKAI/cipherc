using cipherc.Parser;
using cipherc.Utils;
using System;
using System.Diagnostics;
using System.Text;

namespace cipherc
{
    internal class Program
    {
        private static int MainFunction(string[] args)
        {
            var runner = new Parser.Runner();
            if (args.Length == 0)
            {
                runner.IntoShell();
            }
            else
            {
                return runner.RunCommand(args);
            }
            return 0;
        }

        private static int Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                return MainFunction(args);
            }
            else
            {
                try
                {
                    return MainFunction(args);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"{ex.Message}");
                    if (ENV.DEBUG)
                        Console.Error.WriteLine($"{ex.StackTrace}");
                    return 1;
                }
            }
        }
    }
}
