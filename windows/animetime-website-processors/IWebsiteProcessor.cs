using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities;
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

        Task<(string animeUrl, string animeDubUrl)> GetAnimeUrlAsync(string animeTitle, int? releaseYear, IEnumerable<string> animeAltTitles);
        Task<IEnumerable<AnimeSearchResult>> SearchAnimesAsync(string searchString);        

        Task<IEnumerable<(float epNum, string epUrl)>> GetAnimeEpisodesAsync(string animeUrl);
    }
}
