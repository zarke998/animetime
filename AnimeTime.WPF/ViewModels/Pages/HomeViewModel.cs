using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels.Pages
{
    public class HomeViewModel : CommonViewModelBase
    {
        public HomeViewModel(IWindowService windowService, IViewModelLocator viewModelLocator) : base(windowService, viewModelLocator)
        {

        }
    }
}
