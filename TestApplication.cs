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

            LogGroup.Log("Resolving animes: \n");
            foreach (var animeResolve in resolves)
            {
                var anime = _repo.Resolve(animeResolve);
                //Console.WriteLine(anime.Title);
                //Console.WriteLine(anime.TitleAlt);
                //Console.WriteLine(anime.Description);
                //Console.WriteLine(anime.ReleaseYear);
                //Console.WriteLine(anime.YearSeason != null ? anime.YearSeason.Name : "N/A");
                //Console.WriteLine(anime.Rating);
                //Console.WriteLine(anime.Category.Name);
                //foreach(var genre in anime.Genres)
                //    Console.WriteLine(genre.Name);

                //Console.WriteLine("------------------------------------------------------------------------------------------------\n\n\n");

                resolvedCount++;
            }

            LogGroup.Log($"\nResolving finished. Total: {resolvedCount}");

            Console.ReadLine();
        }
    }
}
