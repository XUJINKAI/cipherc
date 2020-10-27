using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.Interpret;
using CipherTool.Transform;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public class TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestBase(ITestOutputHelper output)
        {
            _testOutputHelper = output;
        }

        public string Endline { get; } = "\n";

        protected void TestOutput(string arg, Action<string[], string> AssertFunc)
        {
            _testOutputHelper.WriteLine("Running command...");
            _testOutputHelper.WriteLine(arg);
            _testOutputHelper.WriteLine("");

            var outputStream = new MemoryStream();
            var errorStream = new MemoryStream();

            var setting = new Setting()
            {
                EndOfLine = Endline,
                OutputStream = new StreamWriter(outputStream) { AutoFlush = true },
                ErrorStream = new StreamWriter(errorStream) { AutoFlush = true },
            };
            var args = NativeMethods.CommandLineToArgs(arg);

            var inter = new Interpreter(setting);
            inter.Interpret(args);

            outputStream.Position = 0;
            errorStream.Position = 0;
            var output = new StreamReader(outputStream).ReadToEnd();
            var error = new StreamReader(errorStream).ReadToEnd();

            _testOutputHelper.WriteLine("Output:");
            _testOutputHelper.WriteLine(output);

            _testOutputHelper.WriteLine("Error:");
            _testOutputHelper.WriteLine(error);

            var lines = output.Split(Endline);
            AssertFunc.Invoke(lines, error);
        }
    }
}
