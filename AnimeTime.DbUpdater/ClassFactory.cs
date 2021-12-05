using AnimeTime.Core;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Persistence;
using AnimeTime.Utilities.Core;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
using AnimeTimeDbUpdater.Persistence.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater
{
    public static class ClassFactory
    {
        public static IUnitOfWork CreateUnitOfWork() => new UnitOfWork(new AnimeTimeDbContext());
        public static IEnumMapper<CategoryId> CreateCategoryMapper() => new AnimePlanetCategoryMapper();
        public static IEnumMapper<CharacterRoleId> CreateCharacterRoleMapper() => new AnimePlanetCharacterRoleMapper();
    }
}
