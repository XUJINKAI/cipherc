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

        protected void TestOutput(string arg, Action<string, string> AssertFunc)
        {
            TestOutputHelper.WriteLine("Running command...");
            TestOutputHelper.WriteLine(arg);
            TestOutputHelper.WriteLine("");

            var parseStream = new MemoryStream();
            var evalStream = new MemoryStream();

            var parseSetting = new ParseSetting()
            {
                OutputStream = new StreamWriter(parseStream) { AutoFlush = true },
            };
            var evalSetting = new EvalSetting()
            {
                OutputStream = new StreamWriter(evalStream) { AutoFlush = true },
            };

            var args = Cli.NativeMethods.CommandLineToArgs(arg);
            var block = Parser.Parse(args, parseSetting);
            block.Eval(evalSetting);

            parseStream.Position = 0;
            evalStream.Position = 0;
            var parseOutput = new StreamReader(parseStream).ReadToEnd();
            var evalOutput = new StreamReader(evalStream).ReadToEnd();

            TestOutputHelper.WriteLine("Parse Output:");
            TestOutputHelper.WriteLine(parseOutput);
            TestOutputHelper.WriteLine("");

            TestOutputHelper.WriteLine("Eval Output:");
            TestOutputHelper.WriteLine(evalOutput);
            TestOutputHelper.WriteLine("");

            AssertFunc.Invoke(parseOutput, evalOutput);
        }
    }
}
