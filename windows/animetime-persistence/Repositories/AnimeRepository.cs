﻿using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.Repositories
{
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }
        public AnimeRepository(AnimeTimeDbContext context) : base(context)
        {
        }

        public IEnumerable<string> GetAllTitles()
        {
            return AnimeTimeDbContext.Animes.Select(a => a.Title).ToList();
        }
    }
}