using AnimeTime.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.WebsiteProcessors
{
    public interface IWebsiteProcessor
    {
        (string animeUrl, string animeDubUrl) GetAnimeUrl(string animeName, int releaseYear, string animeAltTitle = null);
        IEnumerable<(string Title, string Url, int releaseYear)> SearchAnimes(string searchString);

        IEnumerable<(float epNum, string epUrl)> GetEpisodes(string animeUrl);
    }
}
