using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Web;
using System.Diagnostics;
using HtmlAgilityPack;
using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Comparers;
using AnimeTime.Persistence;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Persistence;
using AnimeTimeDbUpdater.Utilities;

namespace AnimeTimeDbUpdater
{
    class MainApplication : IApplication
    {
        IAnimeRepositoryFetcher _repo;
        HashSet<string> _titles;
        HashSet<Genre> _genres;
        HashSet<YearSeason> _yearSeasons;
        HashSet<Category> _categories;

        public MainApplication(IAnimeRepositoryFetcher repo)
        {
            _repo = repo;
        }

        public void Run()
        {
            using (IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork())
            {
                _titles = new HashSet<string>(unitOfWork.Animes.GetAllTitles());
            }

            if (_repo.CanFetchByDateAdded)
                UpdateDatabaseViaDate();
            else
                UpdateDatabase();

            Console.ReadLine();
        }
        private void UpdateDatabase()
        {
            var resolves = _repo.GetAllAnimeInfoResolvables();
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

            using (IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork())
            {
                foreach (var anime in resolved)
                {
                    unitOfWork.Animes.Add(anime);
                }

                unitOfWork.Complete();
            }
        }
        private void UpdateDatabaseViaDate()
        {
            var newAnimes = GetNewAnimes();
#if DEBUG
            Console.WriteLine("Reversing anime list.");
            Stopwatch s = new Stopwatch();
            s.Start();
#endif
            newAnimes.Reverse();
#if DEBUG
            s.Stop();
            Console.WriteLine($"Reversing done. Elapsed:{s.ElapsedMilliseconds} ms.");
#endif
            InsertAnimesIntoDatabase(newAnimes);
        }
        private IEnumerable<AnimeInfoResolvable> GetNewAnimes()
        {
            ICollection<AnimeInfoResolvable> newAnimes = new List<AnimeInfoResolvable>();

            string nextPage = null;
            var endOfFetching = false;
            do
            {
                IEnumerable<AnimeInfoResolvable> resolvables = _repo.GetAnimeInfoResolvablesByDateAdded(nextPage);
                nextPage = _repo.NextPage();

                foreach (var r in resolvables)
                {
                    if (_titles.Contains(r.Anime.Title))
                    {
                        endOfFetching = true;
                        break;
                    }
                    newAnimes.Add(r);
                }

            if (nextPage == null)
                endOfFetching = true;

        } while (!endOfFetching);

            return newAnimes;
        }
        private void InsertAnimesIntoDatabase(IEnumerable<AnimeInfoResolvable> animes)
        {
            foreach (var a in animes)
            {
                IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork();
                unitOfWork.InsertOptimizationEnabled = true;

                Anime anime = _repo.Resolve(a);

                _genres = new HashSet<Genre>(unitOfWork.Genres.GetAll(), new GenreComparer());
                _yearSeasons = new HashSet<YearSeason>(unitOfWork.YearSeasons.GetAll(), new YearSeasonComparer());
                _categories = new HashSet<Category>(unitOfWork.Categories.GetAll(), new CategoryComparer());
                AddAnimeRelationships(anime);

                unitOfWork.Animes.Add(anime);
#if DEBUG
                Console.WriteLine("Inserting into database.");
                Stopwatch s1 = new Stopwatch();
                s1.Start();
#endif
                unitOfWork.Complete();
#if DEBUG
                s1.Stop();
                Console.WriteLine($"Inserting done. Elapsed {s1.ElapsedMilliseconds} ms.");
#endif
            }            
        }
        private void AddAnimeRelationships(Anime anime)
        {
            AddAnimeGenresRelationship(anime);
            AddAnimeYearSeasonRelationship(anime);
            AddAnimeCategoryRelationship(anime);
        }
        private void AddAnimeGenresRelationship(Anime anime)
        {
            ICollection<Genre> genres = new List<Genre>();
            foreach (var g in anime.Genres)
            {
                if (_genres.TryGetValue(g, out Genre genre))
                    genres.Add(genre);
                else
                    genres.Add(g);
            }
            anime.Genres = genres;
        }
        private void AddAnimeYearSeasonRelationship(Anime anime)
        {
            if (anime.YearSeason == null) return;

            if (_yearSeasons.TryGetValue(anime.YearSeason, out YearSeason y))
                anime.YearSeason = y;
        }
        private void AddAnimeCategoryRelationship(Anime anime)
        {
            if (anime.Category == null) return;

            if (_categories.TryGetValue(anime.Category, out Category c))
                anime.Category = c;
        }
    }
}
