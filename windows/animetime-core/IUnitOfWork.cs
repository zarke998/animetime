using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Repositories;

namespace AnimeTime.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimeRepository Animes { get; }
        ICategoryRepository Categories { get; }
        IYearSeasonRepository YearSeasons { get; }
        IGenreRepository Genres { get; }
        ICharacterRepository Characters { get; }
        ICharacterRoleRepository CharacterRoles { get; }
        IImageLodLevelRepository ImageLodLevels { get; }
        IAnimeImageRepository AnimeImages{ get; }
        IEpisodeRepository Episodes { get; }
        IAnimeMetadataRepository AnimeMetadatas { get; }
        IWebsiteRepository Websites { get; }
        IAnimeSourceRepository AnimeSources { get; }
        IMetadataRepository Metadata { get; }

        bool InsertOptimizationEnabled { get; set; }

        void Complete();
    }
}

