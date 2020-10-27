using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class ExpectedMoreTokenException : GeneralException
    {
        public override string Message { get; } = "Expect more tokens.";
    }
}
