using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services.Interfaces
{
    public interface IWindowService
    {
        void Load<T>(T viewModel) where T : ViewModelBase;
    }
}
