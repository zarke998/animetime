using AnimeTime.Services.DTO;
using AnimeTime.WPF.Commands;
using AnimeTime.WPF.Models;
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
        private ViewModelBase _activePage;
        private string _activeTab;

        private bool _isWaitingForSearch;
        private ObservableCollection<Tab> _pagesViewModels = new ObservableCollection<Tab>();

        // Services
        private readonly IViewModelLocator _viewModelLocator;
        private readonly ISearchService _searchService;
        private readonly IAnimeService _animeService;

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

        public ObservableCollection<Tab> PagesViewModels { get => _pagesViewModels; set { _pagesViewModels = value; OnPropertyChanged(); } }
        public ViewModelBase ActivePage { get => _activePage; set { _activePage = value; OnPropertyChanged(); } }
        public string ActiveTab { get => _activeTab; set { _activeTab = value; OnPropertyChanged(); } }

        // Commands
        public ICommand SearchAnimeCommand { get; set; }
        public ICommand ShowAnimeCommand { get; set; }
        public ICommand LoadPageCommand { get; set; }
        #endregion


        public MainWindowViewModel(IWindowService windowService,
                                   IViewModelLocator viewModelLocator,
                                   HomeViewModel homeViewModel,
                                   ISearchService searchService,
                                   IAnimeService animeService) : base(windowService, viewModelLocator)
        {
            PagesViewModels.Add(new Tab() { Title = "Home", Value = homeViewModel });
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
            LoadPageCommand = new DelegateCommand(LoadPage);

            _timer = new Timer()
            {
                AutoReset = false,
                Interval = 1000
            };
            _animeService = animeService;
        }
        private void LoadPage(object pageVM)
        {
            var page = PagesViewModels.FirstOrDefault(p => p.Value.GetType() == pageVM.GetType());
            if (page != null)
            {
                ActivePage = page.Value as ViewModelBase;
                ActiveTab = page.Title;
            }
        }
        private async void ShowAnime(object obj)
        {
            var animeId = (int)obj;
            var anime = await _animeService.GetAnimeShort(animeId);

            RemoveDetailsViewFromTabs();

            var detailsViewModel = _viewModelLocator.DetailsViewModel;
            await detailsViewModel.Load(animeId);

            await Task.Delay(1000).ContinueWith(t =>
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    PagesViewModels.Add(new Tab() { Title = anime.Title, Value = detailsViewModel });
                    ActiveTab = anime.Title;
                    ActivePage = detailsViewModel;
                });
            });
        }
        private void SearchAnimes(object obj)
        {

            var searchString = obj.ToString();
            if (String.IsNullOrEmpty(searchString))
            {
                _timer.Stop();
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

        private void RemoveDetailsViewFromTabs()
        {
            var loadedDetailsVM = PagesViewModels.FirstOrDefault(p => p.Value is DetailsViewModel);
            if (loadedDetailsVM != null)
            {
                PagesViewModels.Remove(loadedDetailsVM);
                if(ActiveTab == loadedDetailsVM.Title)
                    ActiveTab = "";
            }
        }
    }
}
