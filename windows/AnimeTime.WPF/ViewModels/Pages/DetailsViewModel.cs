using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels.Pages
{
    public class DetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Genre> Genres { get; set; }

        public DetailsViewModel()
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
        }
    }

    public class Genre
    {
        public string Name { get; set; }
    }
}
