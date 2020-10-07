using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public class ExpectedMoreTokenException : GeneralException
    {
        public ExpectedMoreTokenException(string message) : base(message)
        {
        }

        public ExpectedMoreTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExpectedMoreTokenException()
        {
        }
    }
}
