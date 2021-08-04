using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.ViewModels
{
    public class WindowViewModel : ViewModelBase
    {
        public List<string> NavItems { get; set; } = new List<string>() { "Home", "Library", "Discover", "Music" };
    }
}
