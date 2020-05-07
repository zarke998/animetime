using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeTime.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Threading;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimePlanetRepositoryFetcher : IAnimeRepositoryFetcher
    {
        private IAnimeInfoResolveExtractor _animeInfoExtractor;
        private IAnimeInfoResolver _animeInfoResolver;

        static string AnimeListUrl = "https://www.anime-planet.com/anime/all";
        static string WebsiteUrl = "https://www.anime-planet.com";

        public AnimePlanetRepositoryFetcher(IAnimeInfoResolveExtractor animeInfoExtractor, IAnimeInfoResolver animeInfoResolver)
        {
            _animeInfoExtractor = animeInfoExtractor;
            _animeInfoExtractor.Initialize(WebsiteUrl, AnimeListUrl);

            _animeInfoResolver = animeInfoResolver;
        }

        public IEnumerable<AnimeInfoResolve> GetAnimeInfoResolves()
        {
            List<AnimeInfoResolve> animeResolves = new List<AnimeInfoResolve>();

            //while (!_animeInfoExtractor.IsFinished)
            //{
                animeResolves.AddRange(_animeInfoExtractor.GetAnimeInfoResolvesFromPage());
                _animeInfoExtractor.NextPage();
            //}

            return animeResolves;
        }
        public Anime Resolve(AnimeInfoResolve animeInfoResolve)
        {
            var anime =_animeInfoResolver.Resolve(animeInfoResolve);

#if DEBUG
            Console.WriteLine("\n" + anime);

            Console.WriteLine("Genres: \n");
            foreach (var genre in anime.Genres)
                Console.WriteLine("\t" + genre.Name);

            Console.WriteLine("------------------------------------------------------------------------------------------------\n");
#endif
            return anime;
        }
        public IEnumerable<Anime> ResolveRange(IEnumerable<AnimeInfoResolve> animeInfoResolves)
        {
            List<Anime> animeList = new List<Anime>();

            //foreach(var animeResolve in animeInfoResolves)

            return null;

        }
    }
}
