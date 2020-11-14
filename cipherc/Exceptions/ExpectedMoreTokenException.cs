using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class ExpectedMoreTokenException : BaseException
    {
        public const string KeyFragment = "Expect more tokens.";

        public override string Message => $"{KeyFragment}";

    }
}
