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
                    var lineArgs = CommandLineToArgs(line);
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

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
            out int pNumArgs);

        private static string[] CommandLineToArgs(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                return Array.Empty<string>();

            var argv = CommandLineToArgvW(commandLine, out int argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();

            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
