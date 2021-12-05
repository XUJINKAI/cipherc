using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc.Exceptions
{
    public class ParameterLengthException : Exception
    {
        public ParameterLengthException(string message) : base(message)
        {
        }

        public ParameterLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
