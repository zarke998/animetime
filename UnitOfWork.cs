using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core;
using AnimeTime.Core.Repositories.Interfaces;
using AnimeTime.Persistence.Repositories;

namespace AnimeTime.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnimeTimeDbContext _animeTimeDbContext;
        public IAnimeRepository Animes { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IYearSeasonRepository YearSeasons { get; private set; }
        public IGenreRepository Genres { get; private set; }

        public UnitOfWork(AnimeTimeDbContext context)
        {
            _animeTimeDbContext = context;
            Animes = new AnimeRepository(_animeTimeDbContext);
            Categories = new CategoryRepository(_animeTimeDbContext);
            YearSeasons = new YearSeasonRepository(_animeTimeDbContext);
            Genres = new GenreRepository(_animeTimeDbContext);
        }
        public void Complete()
        {
            _animeTimeDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _animeTimeDbContext.Dispose();
        }
    }
}
