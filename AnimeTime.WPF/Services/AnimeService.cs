using AnimeTime.Services.DTO;
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
        public async Task<IEnumerable<EpisodeDTO>> GetEpisodesAsync(int animeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/animes/{animeId}/episodes");
            var result = await _httpClient.SendAsync(request);

            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EpisodeDTO>>(resultJson);
        }
    }
}
