using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels.Pages
{
    public class DetailsViewModel : ViewModelBase
    {
        private double _verticalContentOffset;

        public double VerticalContentOffset
        {
            get { return _verticalContentOffset; }
            set { _verticalContentOffset = value; OnPropertyChanged(); }
        }

    }
}
