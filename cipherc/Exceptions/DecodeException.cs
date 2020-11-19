using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Exceptions
{
    public class DecodeException : BaseException
    {
        public const string KeyFragment = "Decode error, input string is not valid";

        public string DecodeFormat { get; }
        public string InputString { get; }

        public override string Message => $"{KeyFragment} {DecodeFormat} string: {InputString}";
        
        public new Exception? InnerException { get; }

        public DecodeException(string format, string input)
        {
            DecodeFormat = format;
            InputString = input;
        }

        public DecodeException(string format, string input, Exception inner)
        {
            DecodeFormat = format;
            InputString = input;
            InnerException = inner;
        }
    }
}
