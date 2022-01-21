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
using AnimeTime.WPF.Assets;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for AnimeShortInfo.xaml
    /// </summary>
    public partial class AnimeShortInfo : UserControl
    {
        #region Dependency Properties


        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(AnimeShortInfo), new PropertyMetadata(AssetsURIs.DefaultImage));


        #endregion

        public AnimeShortInfo()
        {
            InitializeComponent();
        }
    }
}
