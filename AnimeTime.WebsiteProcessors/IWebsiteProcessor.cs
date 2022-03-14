using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities;
using AnimeTime.WebsiteProcessors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebsiteProcessors
{
    public interface IWebsiteProcessor
    {
        ICrawlDelayer CrawlDelayer { get; set; }

        Task<AnimeSourceSubDub> TryFindAnime(AnimeSearchParams searchParams);
        Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString);

        Task<IEnumerable<EpisodeSource>> GetEpisodesAsync(string animeUrl);
        Task<IEnumerable<string>> GetVideoSourcesAsync(string episodeUrl);
    }
}
