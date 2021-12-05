using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Exceptions
{
    public class ArgumentErrorException : AbstractRunnerException
    {
        public ArgumentErrorException(string message) : base(message)
        {
        }

        public ArgumentErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
