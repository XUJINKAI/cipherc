using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Handler
{
    public static class Validator
    {
        public static void BytesLength(byte[] bytes, int length, string name)
        {
            if (bytes == null || bytes.Length != length)
            {
                throw new ArgumentErrorException($"Argument {name} length must be {length}");
            }
        }
    }
}
