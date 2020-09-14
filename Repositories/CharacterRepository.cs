using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.Repositories
{
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }

        public CharacterRepository(AnimeTimeDbContext context) : base(context)
        {

        }
    }
}
