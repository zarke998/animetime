using AnimeTime.Core.Domain;
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
        public ObservableCollection<Genre> Genres { get; set; }

        public string Synopsis { get; set; } = "KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj KHSAd akhjdhkj";

        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
        public ObservableCollection<Anime> SameFranchise { get; set; } = new ObservableCollection<Anime>();

        public DetailsViewModel(IWindowService windowService, IViewModelLocator viewModelLocator) : base(windowService, viewModelLocator)
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
