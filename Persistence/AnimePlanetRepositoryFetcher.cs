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
using System.Net;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimePlanetRepositoryFetcher : IAnimeRepositoryFetcher
    {
        private IAnimeInfoResolvableExtractor _animeInfoExtractor;
        private IAnimeInfoResolver _animeInfoResolver;
        
        public string AnimeListUrl { get; private set; } = "https://www.anime-planet.com/anime/all";///
        public string AnimeListByDateAddedUrl { get; private set; } = "https://www.anime-planet.com/anime/all?sort=recent&order=desc";
        public string WebsiteUrl { get; private set; } = "https://www.anime-planet.com";

        public bool CanFetchByDateAdded { get; private set; }
        public string CurrentPage { get; private set; }

        public AnimePlanetRepositoryFetcher(IAnimeInfoResolvableExtractor animeInfoExtractor, IAnimeInfoResolver animeInfoResolver)
        {
            _animeInfoExtractor = animeInfoExtractor;
            _animeInfoResolver = animeInfoResolver;
            CanFetchByDateAdded = UrlIsAvailable(AnimeListByDateAddedUrl);
        }
        public Anime Resolve(AnimeInfoResolvable animeInfoResolve)
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
        public IEnumerable<Anime> ResolveRange(IEnumerable<AnimeInfoResolvable> animeInfoResolves)
        {
            List<Anime> animeList = new List<Anime>();

            //foreach(var animeResolve in animeInfoResolves)

            return null;

        }

        public IEnumerable<AnimeInfoResolvable> GetAnimeInfoResolvablesByDateAdded(string page)
        {
            if(!_animeInfoExtractor.IsSessionStarted)
                _animeInfoExtractor.StartExtractSession(AnimeListUrl, WebsiteUrl);

            CurrentPage = page;

            return _animeInfoExtractor.GetResolvablesFromPage(page);
        }
        public IEnumerable<AnimeInfoResolvable> GetAllAnimeInfoResolvables()
        {
            List<AnimeInfoResolvable> animeResolves = new List<AnimeInfoResolvable>();

            var page = AnimeListUrl;

            _animeInfoExtractor.StartExtractSession(page, WebsiteUrl);
            while (!_animeInfoExtractor.IsFinished)
            {
                animeResolves.AddRange(_animeInfoExtractor.GetResolvablesFromPage(page));
                page = _animeInfoExtractor.NextPage();    
            }
            _animeInfoExtractor.EndExtractSession();

            return animeResolves;
        }

        public string NextPage()
        {
            return _animeInfoExtractor.NextPage();
        }
        public void ResetFetcher()
        {
            _animeInfoExtractor.EndExtractSession();
        }

        private bool UrlIsAvailable(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch(Exception e) { return false; }
        }
    }
}
