using AnimeTime.Services.DTO;
using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using AnimeTime.WPF.ViewModels.Pages;
using AnimeTime.WPF.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        #region Members
        private ObservableCollection<Notification> _notifications;
        private ObservableCollection<AnimeSearchDTO> searchResults = new ObservableCollection<AnimeSearchDTO>();
        private ViewModelBase activePage;

        private bool _isWaitingForSearch;

        // Services
        private readonly IViewModelLocator _viewModelLocator;
        private readonly ISearchService _searchService;

        private Timer _timer;
        #endregion

        #region Properties

        public ObservableCollection<AnimeSearchDTO> SearchResults { get => searchResults; set { searchResults = value; OnPropertyChanged(); } }
        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return _notifications;
            }
            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, object> PagesViewModels { get; set; } = new Dictionary<string, object>();
        public ViewModelBase ActivePage { get => activePage; set { activePage = value; OnPropertyChanged(); } }

        // Commands
        public ICommand SearchAnimeCommand { get; set; }
        public ICommand ShowAnimeCommand { get; set; }
        #endregion


        public MainWindowViewModel(IWindowService windowService, IViewModelLocator viewModelLocator, HomeViewModel homeViewModel, ISearchService searchService) : base(windowService, viewModelLocator)
        {
            PagesViewModels.Add("Home", homeViewModel);
            this._viewModelLocator = viewModelLocator;
            ActivePage = homeViewModel;

            Notifications = new ObservableCollection<Notification>()
            {
                new Notification()
                {
                    Title = "Test"
                },
                new Notification()
                {
                    Title = "Test"
                }
            };
            _searchService = searchService;
            SearchAnimeCommand = new DelegateCommand(SearchAnimes);
            ShowAnimeCommand = new DelegateCommand(ShowAnime);

            _timer = new Timer()
            {
                AutoReset = false,
                Interval = 1000
            };
        }

        private void ShowAnime(object obj)
        {
            var animeId = (int)obj;

            var detailsViewModel = _viewModelLocator.DetailsViewModel;
            ActivePage = detailsViewModel;

            detailsViewModel.Load(animeId);
        }

        private void SearchAnimes(object obj)
        {
            var searchString = obj.ToString();
            if (String.IsNullOrEmpty(searchString))
            {
                SearchResults.Clear();
                return;
            }
            else if (searchString == "Search...") return;

            if (_isWaitingForSearch)
            {
                _timer.Stop();
                _timer = new Timer()
                {
                    AutoReset = false,
                    Interval = 1000
                };
            }

            _timer.Start();
            _isWaitingForSearch = true;

            _timer.Elapsed += async (timerObj, args) =>
            {
                var results = _searchService.SearchAsync(searchString).Result;

                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    foreach (var result in results)
                    {
                        SearchResults.Add(result);
                    }
                });

                _isWaitingForSearch = false;
            };
        }
    }
}
