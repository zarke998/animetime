using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels.Base
{
    public abstract class CommonViewModelBase : ViewModelBase
    {
        private IWindowService _windowService;
        private IViewModelLocator _viewModelLocator;

        public ICommand LoadAnimeEpisodeCommand { get; set; }

        public CommonViewModelBase(IWindowService windowService, IViewModelLocator viewModelLocator)
        {
            _windowService = windowService;
            _viewModelLocator = viewModelLocator;

            LoadAnimeEpisodeCommand = new DelegateCommand(LoadAnimeEpisode, null);
        }
        private void LoadAnimeEpisode(object obj)
        {
            _windowService.Load(_viewModelLocator.PlayerWindowViewModel);
        }

    }
}
