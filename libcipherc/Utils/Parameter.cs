using libcipherc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc.Utils
{
    public static class Parameter
    {
        public static void CheckLength(byte[] data, string name, int expectLength)
        {
            if (data.Length != expectLength)
            {
                throw new ParameterLengthException($"Parameter {name} length must be {expectLength} bytes.");
            }
        }
    }
}
