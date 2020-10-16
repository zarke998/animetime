using AnimeTime.Core.Domain;
using AnimeTime.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence.Repositories
{
    public class CharacterRoleRepository : Repository<CharacterRole>, ICharacterRoleRepository
    {
        public AnimeTimeDbContext AnimeTimeDbContext
        {
            get
            {
                return _dbContext as AnimeTimeDbContext;
            }
        }

        public CharacterRoleRepository(AnimeTimeDbContext context) : base(context)
        {

        }
    }
}
