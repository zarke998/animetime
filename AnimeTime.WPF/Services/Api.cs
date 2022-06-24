using AnimeTime.WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services
{
    public class Api : IApi
    {
        protected readonly HttpClient _httpClient;
        public Api()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:9000/"),
            };
            _httpClient.DefaultRequestHeaders.Add("api-key", "ZsamuKFuj7crELCvmPcJv34o0EVfJctrbLxG11y8b7Yd8GESKL8LbRsDtZyk3Gr70PJP9v0zS1LRt7QIfUGwCNf5Kq863fIhkp74");
        }

        public async Task<string> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode) {
                return null;
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}
