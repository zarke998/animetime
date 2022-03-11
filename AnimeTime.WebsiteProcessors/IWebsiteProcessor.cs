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

        Task<AnimeSourceSubDub> GetAnimeUrlAsync(AnimeSearchParams searchParams);
        Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString);

        Task<IEnumerable<(float epNum, string epUrl)>> GetAnimeEpisodesAsync(string animeUrl);
        Task<IEnumerable<string>> GetVideoSourcesForEpisodeAsync(string episodeUrl);
    }
}
