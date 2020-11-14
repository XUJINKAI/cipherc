using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class NoPipeInputException : BaseException
    {
        public const string KeyFragment = "Pipe is empty";

        public override string Message => $"{KeyFragment}, use | to pass value into pipe.";
    }
}
