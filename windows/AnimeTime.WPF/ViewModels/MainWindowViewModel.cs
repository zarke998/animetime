using AnimeTime.WPF.ViewModels.Base;
using AnimeTime.WPF.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<string> NavItems { get; set; } = new List<string>() { "Home", "Library", "Discover", "Music" };

        public MainWindowViewModel()
        {
            SearchResults = new ObservableCollection<SearchResult>()
            {
                new SearchResult()
                {
                    Title = "Anime 1"
                },
                new SearchResult()
                {
                    Title = "Anime 2"
                },
                new SearchResult()
                {
                    Title = "Anime 3"
                }
            };
        }
    }
}
