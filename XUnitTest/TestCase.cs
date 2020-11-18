using System;
using System.Text;
using CipherTool.Exceptions;
using CipherTool.Interpret;

namespace CipherTool.Test
{
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
}
