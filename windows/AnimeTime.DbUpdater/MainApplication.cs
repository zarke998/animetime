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
using AnimeTime.Utilities;

namespace AnimeTimeDbUpdater
{
    class MainApplication : IApplication
    {
        IAnimeInfoRepository _animeRepo;
        ICharacterInfoRepository _charRepo;

        IImageDownloader _imageDownloader;
        IThumbnailGenerator _thumbnailGenerator;

        ICrawlDelayer _crawlDelayer;

        HashSet<string> _titles;

        HashSet<Genre> _genres;
        HashSet<YearSeason> _yearSeasons;
        HashSet<Category> _categories;
        HashSet<Character> _characters;
        HashSet<ImageLodLevel> _lodLevels;

        public MainApplication(IAnimeInfoRepository animeRepo, ICharacterInfoRepository charRepo, IImageDownloader imageDownloader, IThumbnailGenerator thumbnailGenerator, ICrawlDelayer crawlDelayer)
        {
            _animeRepo = animeRepo;
            _charRepo = charRepo;
            this._imageDownloader = imageDownloader;
            this._thumbnailGenerator = thumbnailGenerator;

            _crawlDelayer = crawlDelayer;

            _animeRepo.CrawlDelayer = _crawlDelayer;
            _imageDownloader.CrawlDelayer = _crawlDelayer;
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
            IEnumerable<AnimeBasicInfo> newAnimes;
            if (_animeRepo.CanFetchByDateAdded)
            {
                newAnimes = GetNewAnimesFast();
                newAnimes.Reverse();
            }
            else
            {
                newAnimes = GetNewAnimes();
            }

            InsertAnimes(newAnimes);
        }

        private IEnumerable<AnimeBasicInfo> GetNewAnimes()
        {
            var newAnimes = _animeRepo.GetAll();
            newAnimes = newAnimes.Where(basicInfo => !_titles.Contains(basicInfo.Title));

            return newAnimes;

        }
        private IEnumerable<AnimeBasicInfo> GetNewAnimesFast()
        {
            ICollection<AnimeBasicInfo> newAnimes = new List<AnimeBasicInfo>();

            var endOfFetching = false;

            do
            {
                IEnumerable<AnimeBasicInfo> infos = _animeRepo.GetByDate();

                foreach (var info in infos)
                {
                    if (_titles.Contains(info.Title))
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
        private void InsertAnimes(IEnumerable<AnimeBasicInfo> infos)
        {
            foreach (var basicInfo in infos)
            {
                IUnitOfWork unitOfWork = ClassFactory.CreateUnitOfWork();
                InitializeUnitOfWork(unitOfWork);


                Log.TraceEvent(TraceEventType.Information, 0, $"\nResolving anime: {basicInfo.DetailsUrl}");
                var detailedInfo = _animeRepo.Resolve(basicInfo);

                var anime = new Anime();

                AddAnimeCover(detailedInfo, anime);
                AddAnimeCharacters(detailedInfo, anime);
                AddAnimeRelationships(detailedInfo, anime);
                AddAnimeData(detailedInfo, anime);

                unitOfWork.Animes.Add(anime);

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

        private void AddAnimeData(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            anime.Title = detailedInfo.BasicInfo.Title;
            anime.Description = detailedInfo.Description;
            anime.Rating = detailedInfo.Rating;
            anime.ReleaseYear = detailedInfo.ReleaseYear;
        }
        private void AddAnimeCover(AnimeDetailedInfo infoDetails, Anime anime)
        {
            if (infoDetails.CoverUrl == null) return;

            Log.TraceEvent(TraceEventType.Information, 0, "Downloading cover image...");
            var coverImage = _imageDownloader.Download(infoDetails.CoverUrl);

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
            animeImage.Image = image;

            anime.Images.Add(animeImage);

            Log.TraceEvent(TraceEventType.Information, 0, "Attached cover to anime.");
            #endregion
        }
        private void AddAnimeCharacters(AnimeDetailedInfo info, Anime anime)
        {
            ICollection<Character> chars = new List<Character>();
            var newChars = new List<(CharacterDetailedInfo detailedInfo, Character character)>();

            Log.TraceEvent(TraceEventType.Information, 0, "\nFetching characters list.");
            var charBasicInfos = _charRepo.Extract(info.CharactersUrl);
            Log.TraceEvent(TraceEventType.Information, 0, $"Characters found ({charBasicInfos.Count()}).");

            if (charBasicInfos.Count() == 0) return;

            foreach (var basicInfo in charBasicInfos)
            {
                var character = new Character();
                character.SourceUrl = basicInfo.DetailsUrl;

                if (_characters.TryGetValue(character, out Character c))
                {
                    chars.Add(c);
                }
                else
                {
                    Log.TraceEvent(TraceEventType.Information, 0, $"Resolving character: {basicInfo.DetailsUrl}.");
                    var detailedInfo = _charRepo.Resolve(basicInfo);

                    var roleMapper = ClassFactory.CreateCharacterRoleMapper();
                    var roleId = roleMapper.Map(detailedInfo.BasicInfo.Role);

                    character.Name = detailedInfo.Name;
                    character.RoleId = roleId;
                    character.SourceUrl = detailedInfo.BasicInfo.DetailsUrl;

                    chars.Add(character);

                    newChars.Add((detailedInfo, character));
                }
            }
            Log.TraceEvent(TraceEventType.Information, 0, "Attached characters to anime.\n");

            if (newChars.Count > 0)
            {
                AddCharacterImages(newChars);
            }

            anime.Characters = chars;
        }
        private void AddCharacterImages(IEnumerable<(CharacterDetailedInfo DetailedInfo, Character Character)> newChars)
        {
            var charImagePairs = new List<(
                Character Character,
                Image<Rgba32> Image,
                List<(ThumbnailUtil GeneratedThumb, string UploadedUrl)> Thumbnails
                )>();
            // Generate thumbnails
            IList<Task<IEnumerable<ThumbnailUtil>>> thumbnailGenerationTasks = new List<Task<IEnumerable<ThumbnailUtil>>>();

            var newCharsWithImage = newChars.Where(pair => pair.DetailedInfo.ImageUrl != null);
            foreach (var newChar in newCharsWithImage)
            {
                Log.TraceEvent(TraceEventType.Information, 0, $"Donwloading image for {newChar.DetailedInfo.Name }.");
                var image = _imageDownloader.Download(newChar.DetailedInfo.ImageUrl);

                charImagePairs.Add((newChar.Character, image, new List<(ThumbnailUtil GeneratedThumb, string UploadedUrl)>()));

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
                        .ContinueWith(t => charImagePairs[indexCopy].Thumbnails.Add((thumbnail, t.Result))));
                }
            }

            Task.WhenAll(uploadTasks).Wait();
            Log.TraceEvent(TraceEventType.Information, 0, $"Uploading finished in: {stopwatch.ElapsedMilliseconds}ms");


            // Insert thumbnails into db
            foreach (var pair in charImagePairs)
            {
                var character = pair.Character;
                var imageOriginal = pair.Image;
                var thumbUrlPairs = pair.Thumbnails;

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
        private void AddAnimeRelationships(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            AddAnimeAltTitles(detailedInfo, anime);

            AddAnimeGenresRelationship(detailedInfo, anime);
            AddAnimeYearSeasonRelationship(detailedInfo, anime);
            AddAnimeCategoryRelationship(detailedInfo, anime);
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

        private void AddAnimeAltTitles(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            foreach(var altTitle in detailedInfo.AltTitles)
            {
                anime.AltTitles.Add(new AnimeAltTitle() { Title = altTitle });
            }
        }
        private void AddAnimeGenresRelationship(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            foreach (var genreName in detailedInfo.Genres)
            {
                var genre = new Genre() { Name = genreName };

                if (_genres.TryGetValue(genre, out Genre existingGenre))
                    anime.Genres.Add(existingGenre);
                else
                    anime.Genres.Add(genre);
            }
        }
        private void AddAnimeYearSeasonRelationship(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            if (detailedInfo.YearSeason == null) return;

            var yearSeason = new YearSeason() { Name = detailedInfo.YearSeason };
            if (_yearSeasons.TryGetValue(yearSeason, out YearSeason existingYearSeason))
            {
                anime.YearSeason = existingYearSeason;
            }
            else
            {
                anime.YearSeason = yearSeason;
            }
        }
        private void AddAnimeCategoryRelationship(AnimeDetailedInfo detailedInfo, Anime anime)
        {
            if (detailedInfo.Category == null) return;

            var category = new Category() { Name = detailedInfo.Category };
            if (_categories.TryGetValue(category, out Category c))
            {
                anime.Category = c;
            }
            else
            {
                anime.Category = category;

                var mapper = ClassFactory.CreateCategoryMapper();
                var catId = mapper.Map(detailedInfo.Category);

                if(catId != CategoryId.Other)
                {
                    category.Id = (int)catId;
                }
                else
                {
                    var maxId = _categories.Select(cat => cat.Id).Max();

                    var lastEnumValue = Enum.GetValues(typeof(CategoryId)).Length - 1;
                    
                    var nextId = maxId < lastEnumValue ? lastEnumValue: maxId;
                    nextId++;

                    anime.Category.Id = nextId;
                }
            }
        }
    }
}