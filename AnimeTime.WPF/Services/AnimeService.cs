﻿using AnimeTime.Services.DTO;
using AnimeTime.WPF.Services.Base;
using AnimeTime.WPF.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services
{
    public class AnimeService : ApiServiceBase, IAnimeService
    {
        private readonly IApi _api;

        public AnimeService(IApi api)
        {
            this._api = api;
        }
        public async Task<IEnumerable<EpisodeDTO>> GetEpisodesAsync(int animeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/animes/{animeId}/episodes");
            var result = await _httpClient.SendAsync(request);

            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EpisodeDTO>>(resultJson);
        }

        public async Task<AnimeDTO> GetAnimeShort(int id)
        {
            var animeJson = await _api.GetAsync($"api/animes/{id}/short-info");

            return JsonConvert.DeserializeObject<AnimeDTO>(animeJson);
        }

        public async Task<AnimeLongDTO> GetAnimeLong(int id)
        {
            var animeJson = await _api.GetAsync($"api/animes/{id}/long-info");

            return JsonConvert.DeserializeObject<AnimeLongDTO>(animeJson);
        }
    }
}
