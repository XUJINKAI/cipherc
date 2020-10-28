using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class NoPipeInputException : GeneralException
    {
        public override string Message { get; } = "Pipe is empty, use | to pass value into pipe.";
    }
}
