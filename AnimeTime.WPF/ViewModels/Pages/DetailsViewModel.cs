using AnimeTime.Services.DTO;
using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Models;
using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels.Pages
{
    public class DetailsViewModel : CommonViewModelBase
    {

        #region Members
        //Services
        private readonly IAnimeService _animeService;
        private readonly IWindowService _windowService;
        private readonly IViewModelLocator _viewModelLocator;

        //Data
        private Anime _anime = new Anime();
        #endregion

        #region Properties
        public Anime Anime { get => _anime; set { _anime = value; OnPropertyChanged(); } }
        #endregion

        public ICommand LoadEpisodeCommand { get; set; }
        public DetailsViewModel(IWindowService windowService,
                                IViewModelLocator viewModelLocator,
                                IAnimeService animeService) : base(windowService, viewModelLocator)
        {
            this._windowService = windowService;
            this._viewModelLocator = viewModelLocator;
            this._animeService = animeService;
            LoadEpisodeCommand = new DelegateCommand(LoadEpisode);
        }

        private void LoadEpisode(object param)
        {
            var epId = (int)param;
            var playerVM = _viewModelLocator.PlayerWindowViewModel;
            playerVM.Clear();

            playerVM.Load(epId);
            _windowService.Load(playerVM);
        }
        public async Task Load(int animeId)
        {
            var anime = await _animeService.GetAnimeLong(animeId);

            Anime.Title = anime.Title;
            Anime.Cover = anime.Images.FirstOrDefault(i => i.ImageType == "Cover")
                         .Thumbnails.FirstOrDefault(t => t.ImageLodLevel == "Medium")
                         .Url;
            Anime.Rating = Math.Round(anime.Rating, 2);

            Anime.Category = anime.Category;
            if (anime.AltTitles.Count > 0)
                Anime.AlternativeTitles = String.Join(", ", anime.AltTitles);
            Anime.YearSeason = $"{anime.YearSeason} {anime.ReleaseYear}".Trim();

            Anime.Genres = new ObservableCollection<Genre>(anime.Genres.Select(g => new Genre() { Name = g.Name }));
            Anime.Synopsis = anime.Description;

            var episodes = await _animeService.GetEpisodesAsync(animeId);
            Anime.Episodes = new ObservableCollection<EpisodeDTO>(episodes);

            Anime.Characters = new ObservableCollection<Character>(anime.Characters.Select(c => new Character()
            {
                Title = c.Name,
                Image = c.Image?.Thumbnails.FirstOrDefault(t => t.ImageLodLevel == "Medium")?.Url
            }));
        }
    }
}
