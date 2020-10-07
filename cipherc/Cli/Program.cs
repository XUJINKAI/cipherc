using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CipherTool.Parse;

namespace CipherTool.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parseSetting = new ParseSetting();
            var evalSetting = new EvalSetting();
            if (args.Length == 1 && args[0].ToLower(ENV.CultureInfo) == "shell")
            {
                do
                {
                    Console.Write("> ");
                    var line = Console.ReadLine();
                    var lineArgs = NativeMethods.CommandLineToArgs(line);
                    RunCmdArgs(lineArgs, parseSetting, evalSetting);
                } while (true);
            }
            else if (args.Length > 0)
            {
                RunCmdArgs(args, parseSetting, evalSetting);
            }
            else
            {
                ShowHelp();
            }
        }

        static void RunCmdArgs(string[] args, ParseSetting parseSetting, EvalSetting evalSetting)
        {
            try
            {
                var block = Parser.Parse(args, parseSetting);
                block.Eval(evalSetting);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("some help");
        }

    }
}
