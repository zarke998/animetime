using AnimeTime.Core;
using AnimeTime.Core.Domain.Mappers;
using AnimeTime.Persistence;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
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
        public static IImageStorage CreateImageStorage() => new AzureImageStorage();
        public static ICategoryMapper CreateCategoryMapper() => new AnimePlanetCategoryMapper();
    }
}
