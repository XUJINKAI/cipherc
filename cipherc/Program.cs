using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CipherTool.Parse;

namespace CipherTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parseSetting = new ParseSetting();
            if (args.Length == 1 && args[0].ToLower(ENV.CultureInfo) == "shell")
            {
                do
                {
                    Console.Write("> ");
                    var line = Console.ReadLine();
                    var lineArgs = NativeMethods.CommandLineToArgs(line);
                    RunCmdArgs(lineArgs, parseSetting);
                    Console.WriteLine();
                    Console.WriteLine();
                } while (true);
            }
            else if (args.Length > 0)
            {
                RunCmdArgs(args, parseSetting);
            }
            else
            {
                ShowHelp();
            }
        }

        static void RunCmdArgs(string[] args, ParseSetting parseSetting)
        {
            try
            {
                Parser.Eval(args, parseSetting);
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
