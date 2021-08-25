using AnimeTime.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnimeTime.WPF.ViewModels
{
    public class WindowViewModel : ViewModelBase
    {
        public ICommand TabClickCommand { get; set; } = new DelegateCommand(tab => MessageBox.Show(tab.ToString()));
        public List<string> NavItems { get; set; } = new List<string>() { "Home", "Library", "Discover", "Music" };
    }
}
