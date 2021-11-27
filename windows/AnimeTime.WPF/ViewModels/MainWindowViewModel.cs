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
        private ObservableCollection<SearchResult> _searchResults;
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

        private ObservableCollection<Notification> _notifications;
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
        public ICommand SearchResultSelectedCommand { get; set; }
        public ICommand TestCommand { get; set; }

        public List<string> NavItems { get; set; } = new List<string>() { "Home", "Library", "Discover", "Music" };
        public ViewModelBase ActiveTab { get; set; } = new HomeViewModel();


        public MainWindowViewModel()
        {
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
