using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CipherTool.AST;
using CipherTool.Interpret;
using CipherTool.Tokenizer;

namespace CipherTool
{
    internal class Program
    {
        static void Main(string[] args)
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
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    IntoShell(interpreter);
                }
                else
                {
                    ShowHelp();
                }
            }
            else if (args.Length == 1)
            {
                switch (args[0].ToLower(ENV.CultureInfo))
                {
                    case "shell":
                        IntoShell(interpreter);
                        break;
                    case "-h":
                    case "--help":
                    case "help":
                        ShowHelp();
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

        static void IntoShell(Interpreter interpreter)
        {
            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var lineArgs = NativeMethods.CommandLineToArgs(line);
                if (lineArgs.Length == 0) { continue; }
                if (lineArgs.Length == 1 && lineArgs[0].ToLower(ENV.CultureInfo) == "help")
                {
                    ShowHelp();
                    continue;
                }
                RunCmdArgs(interpreter, lineArgs);
                Console.WriteLine();
            } while (true);
        }

        static void RunCmdArgs(Interpreter interpreter, string[] args)
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

        static void ShowHelp()
        {
            Console.WriteLine("some help");
        }

    }
}
