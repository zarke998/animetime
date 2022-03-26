using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels
{
    public class PlayerWindowViewModel : WindowViewModelBase
    {
        private readonly IEpisodeService _episodeService;
        private ObservableCollection<string> _sources;

        #region Properties
        public ObservableCollection<string> Sources { get => _sources; set { _sources = value; OnPropertyChanged(); } }
        #endregion

        public PlayerWindowViewModel(IWindowService windowService,
                                    IViewModelLocator viewModelLocator,
                                    IEpisodeService episodeService) : base(windowService, viewModelLocator)
        {
            this._episodeService = episodeService;
        }

        public async void Load(int episodeId)
        {
            var sources = await _episodeService.GetVideoSources(episodeId);
            Sources = new ObservableCollection<string>(sources.Select(source => source.Url));
        }
    }
}
