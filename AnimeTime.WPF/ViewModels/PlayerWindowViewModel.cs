using AnimeTime.WPF.Services.Interfaces;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels
{
    public class PlayerWindowViewModel : WindowViewModelBase
    {
        public PlayerWindowViewModel(IWindowService windowService, IViewModelLocator viewModelLocator) : base(windowService, viewModelLocator)
        {

        }
    }
}
