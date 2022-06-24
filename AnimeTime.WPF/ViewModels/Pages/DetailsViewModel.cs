using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AnimeTime.WPF.Commands;
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
        private readonly IAnimeService _animeService;
        private readonly IWindowService _windowService;
        private readonly IViewModelLocator _viewModelLocator;
        private ObservableCollection<EpisodeDTO> _episodes = new ObservableCollection<EpisodeDTO>();
        #endregion

        #region Properties
        public ObservableCollection<Genre> Genres { get; set; }

        public string Synopsis { get; set; } = "KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj";

        public ObservableCollection<EpisodeDTO> Episodes { get => _episodes; set { _episodes = value; OnPropertyChanged(); } }
        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
        public ObservableCollection<Anime> SameFranchise { get; set; } = new ObservableCollection<Anime>();
        public ICommand LoadEpisodeCommand { get; set; }
        #endregion


        public DetailsViewModel(IWindowService windowService,
                                IViewModelLocator viewModelLocator,
                                IAnimeService animeService) : base(windowService, viewModelLocator)
        {
            Genres = new ObservableCollection<Genre>()
            {
                new Genre()
                {
                    Name = "Action"
                },
                new Genre()
                {
                    Name = "Adventure"
                },
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
                ,
                new Genre()
                {
                    Name = "Adventure"
                }
            };
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
            var episodes = await _animeService.GetEpisodesAsync(animeId);
            Episodes = new ObservableCollection<EpisodeDTO>(episodes);
        }
    }

    public class Genre
    {
        public string Name { get; set; }
    }
}
