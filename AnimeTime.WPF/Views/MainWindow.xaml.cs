using AnimeTime.WPF.Common;
using AnimeTime.WPF.ViewModels;
using AnimeTime.WPF.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeTime.DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase, ICanShutdown
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Shutdown()
        {
            Application.Current.Shutdown();
        }

        private void MainContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as UIElement).Focus();
        }
    }
}