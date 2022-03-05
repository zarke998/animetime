using AnimeTime.Utilities.MathHelpers;
using AnimeTime.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBar : UserControl
    {
        private Thumb _thumb;
        private Border _trackBackground;
        public ProgressBar()
        {
            InitializeComponent();
            this.Loaded += ProgressBar_Loaded;
            Slider.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetThumbColorFromGradientOffset(e.NewValue / 10);
        }

        private void SetThumbColorFromGradientOffset(double offset)
        {
            offset = MathUtil.Clamp(offset, 0, 1);

            var gradient = _trackBackground.Background as LinearGradientBrush;
            UpdateThumbColor(GradientExtensions.GetRelativeColor(gradient.GradientStops, offset));
        }

        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            _thumb = Slider.Template.FindName("Thumb", Slider) as Thumb;
            _trackBackground = Slider.Template.FindName("TrackBackground", Slider) as Border;
            SetThumbColorFromGradientOffset(0);
        }

        private void UpdateThumbColor(Color color)
        {
            _thumb.Background = new SolidColorBrush(color);
        }
    }
}
