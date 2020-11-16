using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.Cli;
using CipherTool.Exceptions;
using CipherTool.Interpret;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test
{
    public static class InOutTestCases
    {
        private static IEnumerable<(string txt, string hex, string base64)> BasicArgs()
        {
            yield return ("1234567890", "31323334353637383930", "MTIzNDU2Nzg5MA==");
            yield return ("!@#ASDzcv()_*u7890", "2140234153447A637628295F2A7537383930", "IUAjQVNEemN2KClfKnU3ODkw");
        }

        private static List<TestCase> BasicCases(string txt, string hex, string base64) => new List<TestCase>()
        {
            new TestCase($"txt {txt}", hex),
            new TestCase($"txt {txt} print txt", txt),
            new TestCase($"txt {txt} print hex", hex),
            new TestCase($"txt {txt} print base64", base64),

            new TestCase($"hex {hex}", hex),
            new TestCase($"hex {hex} print txt", txt),
            new TestCase($"hex {hex} print hex", hex),
            new TestCase($"hex {hex} print base64", base64),

            new TestCase($"base64 {base64}", hex),
            new TestCase($"base64 {base64} print txt", txt),
            new TestCase($"base64 {base64} print hex", hex),
            new TestCase($"base64 {base64} print base64", base64),
        };

        private static List<TestCase> FormatCases(string txt, string hex, string base64) => new List<TestCase>()
        {
            //new TestCase($"hex {hex} encode base64")
        };

        private static List<TestCase> ConstCases() => new List<TestCase>()
        {
            // NonAscii
            new TestCase($"hex 31323301FF print txt",  "123\x01?"),

            // DataSource
            new TestCase("file tmp", "31323334353637383930"){ PreAction = _ => File.WriteAllText("tmp", "1234567890") },
            new TestCase("var x", "31323334353637383930"){ PreAction = ctx => ctx.Variables.Add("x", "1234567890") },
            new TestCase("pipe", "31323334353637383930"){ PreAction = _ => ConsoleHelper.MockConsoleInput("1234567890") },

            new TestCase("rand 1 print hex", (lines,error) => Assert.True(lines[0].Length == 2)),
            new TestCase("rand 16 print hex", (lines,error) => Assert.True(lines[0].Length == 32)),
            new TestCase("rand 64 print hex", (lines,error) => Assert.True(lines[0].Length == 128)),

            new TestCase("hex 1234 then hex ABCD", "1234", "ABCD"),
            new TestCase("hex 31323334353637383930 print txt then print hex base64 MTIzNDU2Nzg5MA==", "1234567890", "31323334353637383930"),
            
            // Grammar
            new TestCase("help"),
            new TestCase("vars", (lines, error) => Assert.Empty(lines)),
            new TestCase("vars", "a = 010203FF") { PreAction = ctx => ctx.Variables.Add("a", Data.FromHexString("010203FF")) },
            new TestCase("vars", "a = ABCDEF", "b = 123456", "a234567890 = 123456ABCDEF") { PreAction = ctx =>
            {
                ctx.Variables.Add("a", Data.FromHexString("ABCDEF"));
                ctx.Variables.Add("b", Data.FromHexString("123456"));
                ctx.Variables.Add("a234567890", Data.FromHexString("123456ABCDEF"));
            }},

            // Exception
            new TestCase("hex", typeof(ExpectedMoreTokenException), ExpectedMoreTokenException.KeyFragment),
            new TestCase("print hex txt", typeof(ExpectedMoreTokenException), ExpectedMoreTokenException.KeyFragment),
            new TestCase("unknown token", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("hex 1234 repeat x", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("rand x", typeof(UnexpectedTokenException), UnexpectedTokenException.KeyFragment),
            new TestCase("print hex pipe", typeof(NoPipeInputException), NoPipeInputException.KeyFragment) { PreAction = _ => ConsoleHelper.MockConsoleInput(null) },
        };

        public static IEnumerable<object[]> GetTestCases()
        {
            foreach (var arg in BasicArgs())
                foreach (var testCase in BasicCases(arg.txt, arg.hex, arg.base64))
                    yield return new object[] { testCase };

            foreach (var testCase in ConstCases())
                yield return new object[] { testCase };
        }
    }

    public record TestCase
    {
        public string Input { get; }
        public string[]? Output { get; }
        public TestOutputDelegate? AssertAction { get; }

        public Type? Exception { get; }
        public string[]? ErrorMsgContains { get; }

        public Action<IContext>? PreAction { get; init; }

        #region functions
        public TestCase(string _input, params string[] _output)
        {
            Input = _input;
            Output = _output.Length == 0 ? null : _output;
        }

        public TestCase(string _input, TestOutputDelegate testOutputDelegate)
        {
            Input = _input;
            AssertAction = testOutputDelegate;
        }

        public TestCase(string _input, Type _exception, params string[] _errorContains)
        {
            if (!_exception.IsAssignableTo(typeof(BaseException)))
            {
                throw new ArgumentException();
            }
            Input = _input;
            Exception = _exception;
            ErrorMsgContains = _errorContains.Length == 0 ? null : _errorContains;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"'{Input}'");
            if (Output != null)
            {
                sb.Append($", Output: '{string.Join(",", Output)}'");
            }
            if (AssertAction != null)
            {
                sb.Append(", AssertAction");
            }
            if (Exception != null)
            {
                sb.Append($", throw {Exception.Name}");
            }
            if (ErrorMsgContains != null)
            {
                sb.Append($", Errors: '{string.Join(",", ErrorMsgContains)}'");
            }
            if (PreAction != null)
            {
                sb.Append(", PreAction");
            }
            return sb.ToString();
        }
        #endregion
    }

    public class InOutTest : TestBase
    {
        public InOutTest(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(InOutTestCases.GetTestCases), MemberType = typeof(InOutTestCases))]
        public void Tests(TestCase Input)
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
    }
}
