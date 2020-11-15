using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Web
{
    public static class WebUtils
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

        public static async Task<bool> WebpageExistsAsync(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri(url)
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                var fullResponse = await client.GetAsync(url);

                switch (fullResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK: return true;
                    case System.Net.HttpStatusCode.NotFound: return false;
                    default: return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
