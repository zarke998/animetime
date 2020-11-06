using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Number
{
    public static class Extensions
    {
        public static Dictionary<int, string> _ordinalDictionary = new Dictionary<int, string>();

        static Extensions()
        {
            _ordinalDictionary.Add(1, "first");
            _ordinalDictionary.Add(2, "second");
            _ordinalDictionary.Add(3, "third");
            _ordinalDictionary.Add(4, "fourth");
            _ordinalDictionary.Add(5, "fifth");
            _ordinalDictionary.Add(6, "sixth");
            _ordinalDictionary.Add(7, "seventh");
            _ordinalDictionary.Add(8, "eighth");
            _ordinalDictionary.Add(9, "ninth");
            _ordinalDictionary.Add(10, "tenth");
            _ordinalDictionary.Add(11, "eleventh");
            _ordinalDictionary.Add(12, "twelfth");
            _ordinalDictionary.Add(13, "thirteenth");
            _ordinalDictionary.Add(14, "fourteenth");
            _ordinalDictionary.Add(15, "fifteenth");
            _ordinalDictionary.Add(16, "sixteenth");
            _ordinalDictionary.Add(17, "seventeenth");
            _ordinalDictionary.Add(18, "eighteenth");
            _ordinalDictionary.Add(19, "nineteenth");
            _ordinalDictionary.Add(20, "twentieth");
        }

        /// <summary>
        /// Get ordinal string from number (ex 2 -> Second etc).
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Ordinal string or null if num > 20.</returns>
        public static string OrdinalString(this int num)
        {
            if(_ordinalDictionary.TryGetValue(num, out string value))
            {
                return value;
            }
            return null;
        }
        public static string OrdinalSuffix(this int num)
        {
            var numString = num.ToString();

            if (numString.EndsWith("11") || numString.EndsWith("12") || numString.EndsWith("13")) return "th";
            else if (numString.EndsWith("1")) return "st";
            else if (numString.EndsWith("2")) return "nd";
            else if (numString.EndsWith("3")) return "rd";
            else return "th";
        }

        /// <summary>
        /// Get number from ordinal text.
        /// </summary>
        /// <param name="ordinalText"></param>
        /// <returns>Number or -1 if not found.</returns>
        public static int OrdinalTextToNum(this string ordinalText)
        {
            var searchedPair = _ordinalDictionary.FirstOrDefault(pair => pair.Value == ordinalText.ToLower());

            if (searchedPair.Key == 0) return -1;

            return searchedPair.Key;
        }
    }
}
