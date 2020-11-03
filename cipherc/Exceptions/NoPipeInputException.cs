using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class NoPipeInputException : GeneralException
    {
        public const string FragmentMessage = "Pipe is empty";

        public override string Message { get; } = $"{FragmentMessage}, use | to pass value into pipe.";
    }
}
