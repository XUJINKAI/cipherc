using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumNameAttribute : Attribute
    {
        public IReadOnlyList<string> Names { get; }

        public EnumNameAttribute(params string[] names)
        {
            Names = names;
        }
    }

    public static class EnumReflector
    {
        public static T[] GetEnumValues<T>()
        {
            return (T[])typeof(T).GetEnumValues();
        }
    }
}
