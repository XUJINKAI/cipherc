using System;
using System.ComponentModel;
using System.IO;
using CipherTool.Cli;
using CipherTool.Interpret;
using Xunit.Abstractions;
using Random = CipherTool.Cipher.Random;

namespace CipherTool.Test
{
    public delegate void TestOutputDelegate(string[] output, string error);

    public class TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestBase(ITestOutputHelper output)
        {
            _testOutputHelper = output;
        }

        public string Endline { get; } = "\n";

        protected Data GetRandom(int bytes = 128)
        {
            return Random.RandomBytes(bytes);
        }

        protected void AppendLine(string line)
        {
            _testOutputHelper.WriteLine(line);
        }

        protected void MockConsoleIn(string? text)
        {
#if DEBUG
            ConsoleHelper.MockConsoleInput(text);
#else
            throw new Exception($"MockConsoleIn function only work in Debug mode.");
#endif
        }

        protected void TestOutput(string arg, TestOutputDelegate assertFunc, Action<Context> contextAction = null)
        {
            _testOutputHelper.WriteLine("Running command...");
            _testOutputHelper.WriteLine(arg);
            _testOutputHelper.WriteLine("");

            var outputStream = new MemoryStream();
            var errorStream = new MemoryStream();

            var context = new Context()
            {
                EndOfLine = Endline,
                OutputStream = new StreamWriter(outputStream) { AutoFlush = true },
                ErrorStream = new StreamWriter(errorStream) { AutoFlush = true },
                ThrowOnException = true,
            };
            contextAction?.Invoke(context);

            var inter = new Interpreter(context);
            Exception? remExp = null;

            try
            {
                inter.Interpret(arg);
            }
            catch (Exception exp)
            {
                remExp = exp;
            }

            outputStream.Position = 0;
            errorStream.Position = 0;
            var output = new StreamReader(outputStream).ReadToEnd();
            var error = new StreamReader(errorStream).ReadToEnd();

            _testOutputHelper.WriteLine("Output:");
            _testOutputHelper.WriteLine(output);

            _testOutputHelper.WriteLine("Error:");
            _testOutputHelper.WriteLine(error);

            var lines = string.IsNullOrEmpty(output) ? Array.Empty<string>() : output.Split(Endline);
            assertFunc?.Invoke(lines, error);

            if (remExp != null)
            {
                _testOutputHelper.WriteLine($"{remExp.Message}{Endline}{remExp.StackTrace}");
                throw remExp;
            }
        }
    }
}
