using AnimeTime.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Repositories
{
    public interface IAnimeRepository : IRepository<Anime>
    {
        IEnumerable<string> GetAllTitles();
        Anime GetWithSources(int id, bool includeWebsites);
        Anime GetWithAltTitles(int id);
        IEnumerable<int> GetIdsWithNoSources();

        IEnumerable<Anime> Search(string searchString);
    }
}
