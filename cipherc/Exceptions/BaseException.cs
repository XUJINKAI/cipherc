using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException()
        {

        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
