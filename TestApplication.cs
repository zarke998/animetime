using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using AnimeTime.Persistence;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
using HtmlAgilityPack;
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

            foreach (var animeResolve in resolves)
            {
                Console.WriteLine(animeResolve.Anime.Title);
                Console.WriteLine("\t" + animeResolve.AnimeDetailsUrl);
                Console.WriteLine("\t" + animeResolve.AnimeCoverThumbUrl);

                var anime = _repo.Resolve(animeResolve);
                Console.WriteLine(HttpUtility.HtmlDecode(anime.Description));

                Console.WriteLine("\n\n\n---------------------------------------------------");
            }

            Console.ReadLine();
        }
    }
}
