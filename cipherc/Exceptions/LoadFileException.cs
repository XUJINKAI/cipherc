using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Exceptions
{
    public class LoadFileException : AbstractRunnerException
    {
        public LoadFileException(string message) : base(message)
        {
        }

        public LoadFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
