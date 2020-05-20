using AnimeTime.Core;
using AnimeTime.Persistence;
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
    }
}
