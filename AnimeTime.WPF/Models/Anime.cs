using AnimeTime.Services.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Models
{
    public class Anime : ModelBase
    {
        #region Members
        private string _title;
        private string _cover = "/Assets/Images/default-image.jpg";
        private double _rating;
        private string _category;
        private string _alternativeTitles;
        private string _yearSeason;
        private string _studio;
        private ObservableCollection<Genre> _genres;
        private string _synopsis;
        private ObservableCollection<EpisodeDTO> _episodes = new ObservableCollection<EpisodeDTO>();
        private ObservableCollection<Character> _characters = new ObservableCollection<Character>();
        #endregion

        #region Properties
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Cover { get => _cover; set { _cover = value; OnPropertyChanged(); } }
        public double Rating { get => _rating; set { _rating = value; } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        public string AlternativeTitles { get => _alternativeTitles; set { _alternativeTitles = value; OnPropertyChanged(); } }
        public string YearSeason { get => _yearSeason; set { _yearSeason = value; OnPropertyChanged(); } }
        public string Studio { get => _studio; set { _studio = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre> Genres { get => _genres; set { _genres = value; OnPropertyChanged(); } }
        public string Synopsis { get => _synopsis; set { _synopsis = value; OnPropertyChanged(); } }


        public ObservableCollection<EpisodeDTO> Episodes { get => _episodes; set { _episodes = value; OnPropertyChanged(); } }
        public ObservableCollection<Character> Characters { get => _characters; set { _characters = value; OnPropertyChanged(); } }
        #endregion
    }
}
