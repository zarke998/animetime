using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Comparers;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeTimeDbUpdater
{
    class MainApplication : IApplication
    {
        IAnimeInfoRepository _animeRepo;
        ICharacterInfoRepository _charRepo;

        HashSet<string> _titles;
        HashSet<Genre> _genres;
        HashSet<YearSeason> _yearSeasons;
        HashSet<Category> _categories;
        HashSet<Character> _characters;

        public MainApplication(IAnimeInfoRepository animeRepo, ICharacterInfoRepository charRepo)
        {
            _animeRepo = animeRepo;
            _charRepo = charRepo;
        }

        public void Run()
        {
            InitializeDatabaseCache();

            UpdateDatabase();

            Console.ReadLine();
        }
        private void InitializeDatabaseCache()
        {
            using (IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork())
            {
                _titles = new HashSet<string>(unitOfWork.Animes.GetAllTitles());
                _genres = new HashSet<Genre>(unitOfWork.Genres.GetAll(), new GenreComparer());
                _yearSeasons = new HashSet<YearSeason>(unitOfWork.YearSeasons.GetAll(), new YearSeasonComparer());
                _categories = new HashSet<Category>(unitOfWork.Categories.GetAll(), new CategoryComparer());
                _characters = new HashSet<Character>(unitOfWork.Characters.GetAll(), new CharacterComparer());
            }
        }

        private void UpdateDatabase()
        {
            IEnumerable<AnimeInfo> newAnimes;
            if (_animeRepo.CanFetchByDateAdded)
            {
                newAnimes = GetNewAnimesFast();
                newAnimes.Reverse();
            }
            else
            {
                newAnimes = GetNewAnimes();
            }

            InsertAnimesAndData(newAnimes);
        }

        private IEnumerable<AnimeInfo> GetNewAnimes()
        {
            var newAnimes = _animeRepo.GetAll();
            newAnimes = newAnimes.Where(a => !_titles.Contains(a.Anime.Title));

            return newAnimes;

        }
        private IEnumerable<AnimeInfo> GetNewAnimesFast()
        {
            ICollection<AnimeInfo> newAnimes = new List<AnimeInfo>();

            var endOfFetching = false;

            do
            {
                IEnumerable<AnimeInfo> infos = _animeRepo.GetByDate();

                foreach (var info in infos)
                {
                    if (_titles.Contains(info.Anime.Title))
                    {
                        endOfFetching = true;
                        break;
                    }
                    newAnimes.Add(info);
                }

                _animeRepo.NextPage();
                if (_animeRepo.LastPageReached)
                {
                    endOfFetching = true;
                }

            } while (!endOfFetching);

            return newAnimes;
        }
        private void InsertAnimesAndData(IEnumerable<AnimeInfo> infos)
        {
            foreach (var info in infos)
            {
                IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork();
                InitializeUnitOfWork(unitOfWork);

                _animeRepo.Resolve(info);
                var anime = info.Anime;

                InsertAnimeCharacters(info);
                InsertAnime(anime, unitOfWork);

                Console.WriteLine("Inserting into database.");
                unitOfWork.Complete();
                Console.WriteLine($"Inserting done.");


                UnitOfWorkToCache(unitOfWork);
            }
        }
        private void InsertAnime(Anime anime, IUnitOfWork unitOfWork)
        {
            AddAnimeRelationships(anime);
            unitOfWork.Animes.Add(anime);
        }
        private void InsertAnimeCharacters(AnimeInfo info)
        {
            ICollection<Character> chars = new List<Character>();

            LogGroup.Log("Getting characters list.");
            var charInfos = _charRepo.Extract(info.CharactersUrl);

            foreach (var charInfo in charInfos)
            {
                if (_characters.TryGetValue(charInfo.Character, out Character c))
                {
                    LogGroup.Log($"Adding character: {c.Name}.");
                    chars.Add(c);
                }
                else
                {
                    LogGroup.Log($"\nResolving character: {charInfo.Character.SourceUrl}.");
                    _charRepo.Resolve(charInfo);
                    var charResolved = charInfo.Character;

                    LogGroup.Log($"Adding character: {charInfo.Character.Name}.\n");
                    chars.Add(charResolved);
                }
            }
            LogGroup.Log("-------------------------------------------------------------------------------------");
            info.Anime.Characters = chars;
        }

        private void UnitOfWorkToCache(IUnitOfWork unitOfWork)
        {
            _genres.UnionWith(unitOfWork.Genres.GetAllCached());
            _yearSeasons.UnionWith(unitOfWork.YearSeasons.GetAllCached());
            _categories.UnionWith(unitOfWork.Categories.GetAllCached());
            _characters.UnionWith(unitOfWork.Characters.GetAllCached());
        }
        private void InitializeUnitOfWork(IUnitOfWork unitOfWork)
        {
            unitOfWork.InsertOptimizationEnabled = false;
            unitOfWork.Genres.AttachRange(_genres);
            unitOfWork.YearSeasons.AttachRange(_yearSeasons);
            unitOfWork.Categories.AttachRange(_categories);
            unitOfWork.Characters.AttachRange(_characters);
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