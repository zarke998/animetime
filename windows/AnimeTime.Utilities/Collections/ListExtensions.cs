using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Collections
{
    public static class ListExtensions
    {
        public static T[] ShiftRight<T>(this T[] source)
        {
            for (int i = source.Length - 2; i >= 0; i--)
            {
                source[i + 1] = source[i];
            }

            return source;
        }

        public static T[] ShiftLeft<T>(this T[] source)
        {
            for (int i = 1; i < source.Length; i++)
            {
                source[i - 1] = source[i];
            }

            return source;
        }

        public static R MaxOrDefault<T, R>(this IEnumerable<T> list, Func<T, R> maxFunc)
        {
            return list.Count() > 0 ? list.Max(maxFunc) : default(R);
        }
    }
}
