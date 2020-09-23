using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core;
using AnimeTime.Core.Repositories;
using AnimeTime.Persistence.Repositories;

namespace AnimeTime.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnimeTimeDbContext _animeTimeDbContext;
        private bool _insertOptimizationEnabled;
        public bool InsertOptimizationEnabled
        {
            get
            {
                return _insertOptimizationEnabled;
            }
            set
            {
                _insertOptimizationEnabled = value;
                _animeTimeDbContext.Configuration.AutoDetectChangesEnabled = !_insertOptimizationEnabled;
            }
        }

        public IAnimeRepository Animes { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IYearSeasonRepository YearSeasons { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public ICharacterRepository Characters { get; private set; }
        public ICharacterRoleRepository CharacterRoles { get; private set; }
        public IImageLodLevelRepository ImageLodLevels { get; private set; }

        public UnitOfWork(AnimeTimeDbContext context)
        {
            _animeTimeDbContext = context;
            Animes = new AnimeRepository(_animeTimeDbContext);
            Categories = new CategoryRepository(_animeTimeDbContext);
            YearSeasons = new YearSeasonRepository(_animeTimeDbContext);
            Genres = new GenreRepository(_animeTimeDbContext);
            Characters = new CharacterRepository(_animeTimeDbContext);
            CharacterRoles = new CharacterRoleRepository(_animeTimeDbContext);
            ImageLodLevels = new ImageLodLevelRepository(_animeTimeDbContext);
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
