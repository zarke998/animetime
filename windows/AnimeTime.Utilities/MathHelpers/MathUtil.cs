using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.MathHelpers
{
    public class MathUtil
    {
        /// <summary>
        /// Clamp any value between min and max.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static TValue Clamp<TValue>(TValue input, TValue min, TValue max) where TValue : IComparable
        {
            if (input.CompareTo(min) < 0) return min;
            if (input.CompareTo(max) > 0) return max;
            return input;
        }

        /// <summary>
        /// Get max from list of parameters.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static TValue Max<TValue>(params TValue[] values) where TValue : IComparable
        {
            return values.Max();
        }

        /// <summary>
        /// Get min from list of parameters
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static TValue Min<TValue>(params TValue[] values) where TValue : IComparable
        {
            return values.Min();
        }
    }
}
