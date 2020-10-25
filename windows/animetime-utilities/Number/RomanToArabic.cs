using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Number
{
    public static class RomanToArabic
    {
        public static int Convert(string roman)
        {
            if (String.IsNullOrEmpty(roman)) throw new ArgumentException("Argument null or empty.");

            roman = roman.ToUpper();

            var romanValues = new SortedDictionary<char, int>()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

            int result = romanValues[roman.Last()];

            for (int i = roman.Length - 2; i >= 0; i--)
            {
                if(romanValues[roman[i + 1]] <= romanValues[roman[i]])
                {
                    result += romanValues[roman[i]];
                }
                else
                {
                    result -= romanValues[roman[i]];
                }
            }

            return result;
        }
    }
}
