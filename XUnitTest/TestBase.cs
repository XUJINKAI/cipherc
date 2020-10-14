using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CipherTool.Parse;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public class TestBase
    {
        private readonly ITestOutputHelper TestOutputHelper;

        public TestBase(ITestOutputHelper output)
        {
            TestOutputHelper = output;
        }

        public string ENDLINE { get; } = "\n";

        protected void TestOutput(string arg, Action<string, string> AssertFunc)
        {
            TestOutputHelper.WriteLine("Running command...");
            TestOutputHelper.WriteLine(arg);
            TestOutputHelper.WriteLine("");

            var outputStream = new MemoryStream();
            var errorStream = new MemoryStream();
            var outputWriter = new StreamWriter(outputStream) { AutoFlush = true };
            var errorWriter = new StreamWriter(errorStream) { AutoFlush = true };
            Log.OutputStream = outputWriter;
            Log.ErrorStream = errorWriter;
            Log.EndOfLine = ENDLINE;

            var parseSetting = new ParseSetting()
            {
            };
            var args = NativeMethods.CommandLineToArgs(arg);

            Parser.Eval(args, parseSetting);

            outputStream.Position = 0;
            errorStream.Position = 0;
            var output = new StreamReader(outputStream).ReadToEnd();
            var error = new StreamReader(errorStream).ReadToEnd();

            TestOutputHelper.WriteLine("Output:");
            TestOutputHelper.WriteLine(output);

            TestOutputHelper.WriteLine("Error:");
            TestOutputHelper.WriteLine(error);

            AssertFunc.Invoke(output, error);
        }
    }
}
