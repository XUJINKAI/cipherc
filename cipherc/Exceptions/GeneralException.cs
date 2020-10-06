using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException(string message) : base(message)
        {
        }

        public GeneralException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public GeneralException()
        {
        }
    }
}
