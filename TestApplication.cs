using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using AnimeTime.Core.Domain;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
using AnimeTimeDbUpdater.Utilities;
using System.IO;
using System.Drawing;
using System.Web;
using AnimeTime.Persistence;
using AnimeTime.Core;

namespace AnimeTimeDbUpdater
{
    class TestApplication : IApplication
    {
        private IAnimeRepositoryFetcher _repo;

        public TestApplication(IAnimeRepositoryFetcher repo)
        {
            _repo = repo;
        }

        public void Run()
        {
            var resolves = _repo.GetAnimeInfoResolves();
            var resolvedCount = 0;


            ICollection<Anime> resolved = new List<Anime>();
            LogGroup.Log("\nResolving animes: \n");
            foreach (var animeResolve in resolves)
            {
                var anime = _repo.Resolve(animeResolve);
                resolved.Add(anime);

                resolvedCount++;
            }
            LogGroup.Log($"\nResolving finished. Total: {resolvedCount}");

            using (IUnitOfWork unitOfWork = new UnitOfWork(new AnimeTimeDbContext()))
            {
                foreach (var anime in resolved)
                {
                    unitOfWork.Animes.Add(anime);
                }

                unitOfWork.Complete();
            }

            Console.ReadLine();
        }
    }
}
