using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Web
{
    public static class UrlUtils
    {
        public static string CombineUrls(params string[] urls)
        {
            if (urls.Length == 0) return String.Empty;
            else if (urls.Length == 1) return urls[0];

            var result = urls[0].TrimStart('/');
            for (int i = 1; i < urls.Length; i++)
            {
                result += "/" + urls[i].Trim('/');
            }

            return result;
        }
    }
}
