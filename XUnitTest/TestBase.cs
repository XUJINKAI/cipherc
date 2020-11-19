using System;
using System.IO;
using System.Linq;
using CipherTool.Cli;
using CipherTool.Interpret;
using Xunit;
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

        protected static TestOutputDelegate EmptyAssert => (lines, error) =>
        {
        };

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

        protected void TestCase(TestCase Input)
        {
            TestCase data = Input;

            void TestAction()
            {
                TestOutput(data.Input, (lines, error) =>
                {
                    if (data.Output != null)
                    {
                        Assert.Equal(data.Output.Length + 1, lines.Length);
                        for (int i = 0; i < data.Output.Length; i++)
                        {
                            Assert.Equal(data.Output[i], lines[i]);
                        }
                    }
                    data.AssertAction?.Invoke(lines, error);
                    if (data.ErrorMsgContains != null)
                    {
                        Assert.True(data.ErrorMsgContains.All(msg => error.Contains(msg)));
                    }
                }, ctx =>
                {
                    data.PreAction?.Invoke(ctx);
                });
            }

            if (data.Exception != null)
            {
                Assert.Throws(data.Exception, () =>
                {
                    TestAction();
                });
            }
            else
            {
                TestAction();
            }
        }

        protected void TestOutput(string arg, TestOutputDelegate assertFunc, Action<Context>? contextAction = null)
        {
            _testOutputHelper.WriteLine("> " + arg);

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

            _testOutputHelper.WriteLine(output);

            if (!string.IsNullOrEmpty(error))
            {
                _testOutputHelper.WriteLine(error);
            }

            var lines = string.IsNullOrEmpty(output) ? Array.Empty<string>() : output.Split(Endline);
            assertFunc?.Invoke(lines, error);

            if (remExp != null)
            {
                _testOutputHelper.WriteLine($"StackTrace:{Endline}{remExp.StackTrace}");
                throw remExp;
            }
        }
    }
}
