using System;
using System.Linq;

namespace CipherTool.Transform
{
    public static class ArrayExtension
    {
        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, index, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(this T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            T[] result = new T[array.Length - index];
            Array.Copy(array, index, result, 0, result.Length);
            return result;
        }

        public static T[] SubStarts0<T>(this T[] array, int len)
        {
            T[] result = new T[len];
            Array.Copy(array, 0, result, 0, len);
            return result;
        }

        public static bool SequanceEqual<T>(this T[]? array, T[]? compare)
        {
            if (array == null || compare == null)
                return array == compare;
            return Enumerable.SequenceEqual(array, compare);
        }
    }
}
