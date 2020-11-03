using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class ExpectedMoreTokenException : GeneralException
    {
        public const string FragmentMessage = "Expect more tokens.";

        public override string Message { get; } = $"{FragmentMessage}";
    }
}
