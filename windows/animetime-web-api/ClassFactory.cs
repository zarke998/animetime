using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeTime.WebAPI
{
    public static class ClassFactory
    {
        public static IUnitOfWork CreateUnitOfWork() => new UnitOfWork(new AnimeTimeDbContext());
    }
}