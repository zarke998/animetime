using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Repositories.Interfaces;

namespace AnimeTime.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimeRepository Animes { get; }
        ICategoryRepository Categories { get; }
        IYearSeasonRepository YearSeasons { get; }
        IGenreRepository Genres { get; }
        ICharacterRepository Characters { get; }

        bool InsertOptimizationEnabled { get; set; }

        void Complete();
    }
}
