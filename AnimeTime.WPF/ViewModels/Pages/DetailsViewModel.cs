using AnimeTime.Core.Domain;
using AnimeTime.Services.DTO;
using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels.Pages
{
    public class DetailsViewModel : CommonViewModelBase
    {
        #region Members
        private readonly IAnimeService _animeService;
        private ObservableCollection<EpisodeDTO> _episodes = new ObservableCollection<EpisodeDTO>();
        #endregion

        #region Properties
        public ObservableCollection<Genre> Genres { get; set; }

        public string Synopsis { get; set; } = "KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj";

        public ObservableCollection<EpisodeDTO> Episodes { get => _episodes; set { _episodes = value; OnPropertyChanged(); } }
        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
        public ObservableCollection<Anime> SameFranchise { get; set; } = new ObservableCollection<Anime>();
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
            this._animeService = animeService;
        }

        public async void Load(int animeId)
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
