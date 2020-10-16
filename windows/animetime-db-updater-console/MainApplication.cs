using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Core.Domain.Comparers;
using AnimeTime.Utilities.Core.Imaging;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThumbnailUtil = AnimeTime.Utilities.Core.Domain.Thumbnail;
using AnimeTime.Utilities.Imaging;
using AnimeTime.Core.Exceptions;
using System.Diagnostics;

namespace AnimeTimeDbUpdater
{
    class MainApplication : IApplication
    {
        IAnimeInfoRepository _animeRepo;
        ICharacterInfoRepository _charRepo;

        IImageDownloader _imageDownloader;
        IThumbnailGenerator _thumbnailGenerator;

        HashSet<string> _titles;

        HashSet<Genre> _genres;
        HashSet<YearSeason> _yearSeasons;
        HashSet<Category> _categories;
        HashSet<Character> _characters;
        HashSet<ImageLodLevel> _lodLevels;

        public MainApplication(IAnimeInfoRepository animeRepo, ICharacterInfoRepository charRepo, IImageDownloader imageDownloader, IThumbnailGenerator thumbnailGenerator)
        {
            _animeRepo = animeRepo;
            _charRepo = charRepo;
            this._imageDownloader = imageDownloader;
            this._thumbnailGenerator = thumbnailGenerator;
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
                _lodLevels = new HashSet<ImageLodLevel>(unitOfWork.ImageLodLevels.GetAll());
            }
        }
        private void UpdateCharacterCache(IEnumerable<Character> newChars)
        {
            _characters.UnionWith(newChars);
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
                try
                {
                    unitOfWork.Complete();
                }
                catch (EntityInsertException insertException)
                {
                    // Log exception to db

                    // Cleanup uploaded images

                    Debug.WriteLine(insertException.Message + insertException.InnerException.Message);

                    Environment.Exit(0);
                }
                Console.WriteLine($"Inserting done.");

                UpdateCharacterCache(anime.Characters);
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
            ICollection<CharacterInfo> newChars = new List<CharacterInfo>();

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

                    newChars.Add(charInfo);
                }
            }

            if (newChars.Count > 0)
            {
                InsertCharacterImages(newChars);
            }

            LogGroup.Log("-------------------------------------------------------------------------------------");
            info.Anime.Characters = chars;
        }

        private void InsertCharacterImages(IEnumerable<CharacterInfo> newChars)
        {
            var charImagePairs = new List<Tuple<
                CharacterInfo,
                Image<Rgba32>,
                List<Tuple<ThumbnailUtil, string>>>>();


            // Generate thumbnails
            IList<Task<IEnumerable<ThumbnailUtil>>> thumbnailGenerationTasks = new List<Task<IEnumerable<ThumbnailUtil>>>();

            var newCharsWithImage = newChars.Where(ci => ci.ImageUrl != null);
            foreach (var c in newCharsWithImage)
            {
                Console.Write($"Donwloading image for {c.Character.Name}."); // Switch to using LogGroup Write method (to be implemented)
                var image = _imageDownloader.Download(c.ImageUrl, true);
                Console.WriteLine("Done");

                charImagePairs.Add(Tuple.Create(c, image, new List<Tuple<ThumbnailUtil, string>>()));

                thumbnailGenerationTasks.Add(_thumbnailGenerator.GenerateAsync(image, _lodLevels));
            }

            Console.Write("Generating thumbnails...");
            var charThumbnails = Task.WhenAll(thumbnailGenerationTasks).Result;
            Console.WriteLine("Done.");

            // Upload thumbnails
            var imageStorage = ClassFactory.CreateImageStorage();

            var uploadTasks = new List<Task>();
            for (int i = 0; i < charThumbnails.Length; i++)
            {
                var indexCopy = i;
                foreach (var thumbnail in charThumbnails[i])
                {
                    uploadTasks.Add(imageStorage.UploadAsync(thumbnail.Image)
                        .ContinueWith(t => charImagePairs[indexCopy].Item3.Add(Tuple.Create(thumbnail, t.Result))));
                }
            }
            Console.Write("Uploading thumbnails...");
            Task.WhenAll(uploadTasks).Wait();
            Console.WriteLine("Done.");

            // Insert thumbnails into db
            foreach (var pair in charImagePairs)
            {
                var character = pair.Item1.Character;
                var imageOriginal = pair.Item2;
                var thumbUrlPairs = pair.Item3;

                var image = new AnimeTime.Core.Domain.Image();
                image.ImageType_Id = ImageTypeId.Character;

                if (imageOriginal.IsPortrait())
                {
                    image.Orientation_Id = ImageOrientationId.Portrait;
                }
                else
                {
                    image.Orientation_Id = ImageOrientationId.Landscape;
                }

                foreach (var thumbUrlPair in thumbUrlPairs)
                {
                    var thumb = new Thumbnail();
                    thumb.ImageLodLevel_Id = _lodLevels.First(lod => lod.Level == thumbUrlPair.Item1.LodLevel).Id;
                    thumb.Url = thumbUrlPair.Item2;

                    image.Thumbnails.Add(thumb);
                }
                character.Image = image;
            }
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