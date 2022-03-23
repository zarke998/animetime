using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels;
using AnimeTime.WPF.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services
{
    public class ViewModelLocator : IViewModelLocator
    {
        public PlayerWindowViewModel PlayerWindowViewModel { get; set; }

        public DetailsViewModel DetailsViewModel { get; set; }
    }
}
