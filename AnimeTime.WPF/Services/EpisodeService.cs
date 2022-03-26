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
    public class EpisodeService : ApiServiceBase, IEpisodeService
    {
        public async Task<IEnumerable<VideoSourceDTO>> GetVideoSources(int episodeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/episodes/{episodeId}/video-sources");
            var result = await _httpClient.SendAsync(request);

            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VideoSourceDTO>>(resultJson);
        }
    }
}
