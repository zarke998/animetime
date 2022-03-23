using AnimeTime.WPF.ViewModels;
using AnimeTime.WPF.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services.Interfaces
{
    public interface IViewModelLocator
    {
        PlayerWindowViewModel PlayerWindowViewModel { get; set; }
        DetailsViewModel DetailsViewModel { get; set; }
    }
}
