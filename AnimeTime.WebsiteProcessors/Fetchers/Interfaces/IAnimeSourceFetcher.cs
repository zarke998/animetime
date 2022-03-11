using AnimeTime.WebsiteProcessors.Models;
using System.Collections.Generic;

namespace AnimeTime.WebsiteProcessors.Interfaces
{
    public interface IAnimeSourceFetcher
    {
        IEnumerable<AnimeSourceSubDub> Fetch(AnimeSearchParams searchParams, IEnumerable<string> websiteNames);
    }
}