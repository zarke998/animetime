using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.String
{
    public static class Extensions
    {
        public static string RemoveExtraWhitespaces(this string value) 
        {
            value = value.Trim();
            var builder = new StringBuilder();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char c = value[i];

                if(!Char.IsWhiteSpace(c))
                {
                    builder.Append(c);
                    continue;
                }
                else if(Char.IsWhiteSpace(c) && !Char.IsWhiteSpace(value[i + 1]))
                {
                    builder.Append(c);
                    continue;
                }
            }
            builder.Append(value[value.Length - 1]);

            return builder.ToString();
        }
    }
}
