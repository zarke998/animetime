using AnimeTime.WPF.Commands;
using AnimeTime.WPF.ViewModels.Base;
using AnimeTime.WPF.ViewModels.Pages;
using AnimeTime.WPF.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        #region Members
        private ObservableCollection<SearchResult> _searchResults;
        private ObservableCollection<Notification> _notifications;
        #endregion

        #region Properties
        public ObservableCollection<SearchResult> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }
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
        public ViewModelBase ActivePage { get; set; } = new DetailsViewModel();

        public ICommand SearchResultSelectedCommand { get; set; }
        #endregion


        public MainWindowViewModel(HomeViewModel homeViewModel)
        {
            PagesViewModels.Add("Home", homeViewModel);

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
        }
    }
}
