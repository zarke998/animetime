using AnimeTime.Services.DTO;
using AnimeTime.WPF.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AnimeTime.WPF.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<IEnumerable<AnimeSearchDTO>> SearchAsync(string searchString)
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:9000/api/search?animeName={searchString}"),
            };
            request.Headers.Add("api-key", "ZsamuKFuj7crELCvmPcJv34o0EVfJctrbLxG11y8b7Yd8GESKL8LbRsDtZyk3Gr70PJP9v0zS1LRt7QIfUGwCNf5Kq863fIhkp74");
            var result = await httpClient.SendAsync(request);

            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AnimeSearchDTO>>(resultJson);
        }
    }
}