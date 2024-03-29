﻿using AnimeTime.Services.DTO;
using AnimeTime.WPF.Services.Base;
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
    public class SearchService : ApiServiceBase, ISearchService
    {
        public async Task<IEnumerable<AnimeSearchDTO>> SearchAsync(string searchString)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/search?animeName={searchString}");            
            var result = await _httpClient.SendAsync(request);

            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AnimeSearchDTO>>(resultJson);
        }
    }
}