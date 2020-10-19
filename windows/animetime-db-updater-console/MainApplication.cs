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


                Log.TraceEvent(TraceEventType.Information, 0, $"\nResolving anime: {info.AnimeDetailsUrl}");
                _animeRepo.Resolve(info);

                var anime = info.Anime;

                InsertAnimeCover(info, unitOfWork);
                InsertAnimeCharacters(info);
                InsertAnime(anime, unitOfWork);

                Log.TraceEvent(TraceEventType.Information, 0, "\nInserting anime into database.");
                try
                {
                    unitOfWork.Complete();
                }
                catch (EntityInsertException insertException)
                {
                    // Log exception to db

                    // Cleanup uploaded images

                    Log.TraceEvent(TraceEventType.Error, 0, insertException.Message);
                    Log.TraceEvent(TraceEventType.Error, 0, insertException.InnerException.Message);

                    Environment.Exit(0);
                }
                Log.TraceEvent(TraceEventType.Information, 0, "-------------------------------------");

                UpdateCharacterCache(anime.Characters);
                UnitOfWorkToCache(unitOfWork);
            }
        }
        private void InsertAnime(Anime anime, IUnitOfWork unitOfWork)
        {
            AddAnimeRelationships(anime);
            unitOfWork.Animes.Add(anime);
        }
        private void InsertAnimeCover(AnimeInfo info, IUnitOfWork unitOfWork)
        {
            if (info.CoverUrl == null)
            {
                return;
            }

            Log.TraceEvent(TraceEventType.Information, 0, "Downloading cover image...");
            var coverImage = _imageDownloader.Download(info.CoverUrl);

            Log.TraceEvent(TraceEventType.Information, 0, "Generating thumbnails...");
            var thumbnails = _thumbnailGenerator.GenerateAsync(coverImage, _lodLevels).Result;

            #region Upload thumbnails
            var imageStorage = ClassFactory.CreateImageStorage();

            var uploadTasks = new List<Task>();
            var urlLodPairs = new List<Tuple<string, LodLevel>>();

            Log.TraceEvent(TraceEventType.Information, 0, "Uploading thumbnails...");
            foreach (var thumb in thumbnails)
            {
                uploadTasks.Add(imageStorage.UploadAsync(thumb.Image).ContinueWith(t => urlLodPairs.Add(Tuple.Create(t.Result, thumb.LodLevel))));
            }
            Task.WhenAll(uploadTasks).Wait();
            #endregion

            #region Insert anime cover into database
            var animeImage = new AnimeImage();

            var image = new AnimeTime.Core.Domain.Image();

            if (coverImage.IsPortrait())
            {
                image.Orientation_Id = ImageOrientationId.Portrait;
            }
            else
            {
                image.Orientation_Id = ImageOrientationId.Landscape;
            }

            image.ImageType_Id = ImageTypeId.Cover;

            foreach (var pair in urlLodPairs)
            {
                var thumbnail = new Thumbnail();

                thumbnail.Url = pair.Item1;
                thumbnail.ImageLodLevel_Id = _lodLevels.First(lod => lod.Level == pair.Item2).Id;

                image.Thumbnails.Add(thumbnail);
            }

            animeImage.Anime = info.Anime;
            animeImage.Image = image;

            unitOfWork.AnimeImages.Add(animeImage);

            Log.TraceEvent(TraceEventType.Information, 0, "Attached cover to anime.");
            #endregion
        }
        private void InsertAnimeCharacters(AnimeInfo info)
        {
            ICollection<Character> chars = new List<Character>();
            ICollection<CharacterInfo> newChars = new List<CharacterInfo>();

            Log.TraceEvent(TraceEventType.Information, 0, "\nFetching characters list.");
            var charInfos = _charRepo.Extract(info.CharactersUrl);
            Log.TraceEvent(TraceEventType.Information, 0, $"Characters found ({charInfos.Count()}).");

            if (charInfos.Count() == 0) return;

            foreach (var charInfo in charInfos)
            {
                if (_characters.TryGetValue(charInfo.Character, out Character c))
                {
                    chars.Add(c);
                }
                else
                {
                    Log.TraceEvent(TraceEventType.Information, 0, $"Resolving character: {charInfo.Character.SourceUrl}.");
                    _charRepo.Resolve(charInfo);

                    var charResolved = charInfo.Character;
                    chars.Add(charResolved);

                    newChars.Add(charInfo);
                }
            }
            Log.TraceEvent(TraceEventType.Information, 0, "Attached characters to anime.\n");

            if (newChars.Count > 0)
            {
                InsertCharacterImages(newChars);
            }

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
                Log.TraceEvent(TraceEventType.Information, 0, $"Donwloading image for {c.Character.Name}.");
                var image = _imageDownloader.Download(c.ImageUrl, true);

                charImagePairs.Add(Tuple.Create(c, image, new List<Tuple<ThumbnailUtil, string>>()));

                thumbnailGenerationTasks.Add(_thumbnailGenerator.GenerateAsync(image, _lodLevels));
            }

            Log.TraceEvent(TraceEventType.Information, 0, "Generating thumbnails...");
            var charThumbnails = Task.WhenAll(thumbnailGenerationTasks).Result;

            // Upload thumbnails
            var imageStorage = ClassFactory.CreateImageStorage();

            var uploadTasks = new List<Task>();

            Log.TraceEvent(TraceEventType.Information, 0, "Uploading thumbnails...");
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < charThumbnails.Length; i++)
            {
                var indexCopy = i;
                foreach (var thumbnail in charThumbnails[i])
                {
                    uploadTasks.Add(imageStorage.UploadAsync(thumbnail.Image)
                        .ContinueWith(t => charImagePairs[indexCopy].Item3.Add(Tuple.Create(thumbnail, t.Result))));
                }
            }

            Task.WhenAll(uploadTasks).Wait();
            Log.TraceEvent(TraceEventType.Information, 0, $"Uploading finished in: {stopwatch.ElapsedMilliseconds}ms");


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
            Log.TraceEvent(TraceEventType.Information, 0, "Attached images to characters.\n");
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