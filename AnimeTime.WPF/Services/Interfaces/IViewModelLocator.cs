using AnimeTime.WPF.ViewModels;
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
    }
}
