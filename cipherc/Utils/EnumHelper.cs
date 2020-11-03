using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace CipherTool.Utils
{
    public static class EnumHelper
    {
        public static T? GetAttribute<T>(this Enum @enum) where T : Attribute
        {
            Contract.Assume(@enum != null);
            var type = @enum.GetType();
            var member = type.GetMember(@enum.ToString()).First();
            var attr = member.GetCustomAttribute(typeof(T));
            return attr as T;
        }

        public static IEnumerable<T> GetEnumValues<T>(Func<T, bool>? predicate = null) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return predicate == null ? values : values.Where(predicate);
        }

        public static string StringJoinEnums<T>(string separator = ", ") where T : Enum
        {
            return string.Join(separator, GetEnumValues<T>());
        }
    }
}