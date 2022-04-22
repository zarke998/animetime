using AnimeTime.WPF.Utility;
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

namespace AnimeTime.WPF.Views.Controls.VideoPlayerControls
{
    /// <summary>
    /// Interaction logic for FullscreenButtonUC.xaml
    /// </summary>
    public partial class FullscreenButtonUC : UserControl
    {

        #region Dependency Properties
        public bool IsFullscreen
        {
            get { return (bool)GetValue(IsFullscreenProperty); }
            set { SetValue(IsFullscreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFullscreen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFullscreenProperty =
            DependencyProperty.Register("IsFullscreen", typeof(bool), typeof(FullscreenButtonUC), new PropertyMetadata(false, IsFullscreenChanged));

        private static void IsFullscreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as FullscreenButtonUC;

            string image;
            if ((bool)e.NewValue)
                image = "/Assets/Icons/fullscreen-on.png";
            else
                image = "/Assets/Icons/fullscreen-off.png";

            self.Icon.SetCurrentValue(Image.SourceProperty, new BitmapImage(new Uri(image, UriKind.Relative)));
        }

        public ICommand FullscreenToggleCommand
        {
            get { return (ICommand)GetValue(FullscreenToggleCommandProperty); }
            set { SetValue(FullscreenToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FullscreenToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FullscreenToggleCommandProperty =
            DependencyProperty.Register("FullscreenToggleCommand", typeof(ICommand), typeof(FullscreenButtonUC), new PropertyMetadata(null));

        #endregion
        public FullscreenButtonUC()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.SetCurrentValue(IsFullscreenProperty, !IsFullscreen);
            FullscreenToggleCommand.TryExecute(IsFullscreen);

            Container.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
