using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Web
{
    public static class UrlUtil
    {
        public static string AddQueryParam(string url, string param, string value)
        {
            if (url == null || param == null || value == null)
                throw new ArgumentNullException();

            return $"{url}&{param}={value}";
        }

        public static bool IsAbsolute(string url)
        {
            if (url == null)
                throw new ArgumentNullException();

            return url.StartsWith("http") || url.StartsWith("https");
        }
    }
}
