using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Web
{
    public static class UrlUtils
    {
        /// <summary>
        /// Combines urls by joining them with '/'.
        /// </summary>
        /// <param name="urls">Urls to join.</param>
        /// <returns>Combined urls.</returns>
        public static string CombineUrls(params string[] urls)
        {
            if (urls.Length == 0) return System.String.Empty;
            else if (urls.Length == 1) return urls[0];

            var result = urls[0].Trim('/');
            for (int i = 1; i < urls.Length; i++)
            {
                result += "/" + urls[i].Trim('/');
            }

            return result;
        }
    }
}
