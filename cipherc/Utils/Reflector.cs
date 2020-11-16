using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Utils
{
    public static class Reflector
    {
        public static T[] GetEnums<T>()
        {
            return (T[])typeof(T).GetEnumValues();
        }
    }
}
