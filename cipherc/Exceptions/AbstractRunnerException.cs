using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Exceptions
{
    public abstract class AbstractRunnerException : Exception
    {
        public AbstractRunnerException(string message) : base(message)
        {
        }

        public AbstractRunnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
