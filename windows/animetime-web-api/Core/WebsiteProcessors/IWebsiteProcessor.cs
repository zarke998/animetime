using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WebAPI.Core.WebsiteProcessors
{
    public interface IWebsiteProcessor
    {
        string GetAnimeUrl(string animeName, string animeAltTitle = null);
        IEnumerable<(string Title, string Url)> SearchAnimes(string searchString);
    }
}
