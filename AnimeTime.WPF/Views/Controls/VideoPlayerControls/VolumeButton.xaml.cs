using AnimeTime.Utilities.MathHelpers;
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
    /// Interaction logic for VolumeButton.xaml
    /// </summary>
    public partial class VolumeButton : UserControl
    {
        private bool valueAnimating;
        #region Dependency Properties
        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }
        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(double), typeof(VolumeButton), new PropertyMetadata(0.0, OnVolumeChanged));


        public ICommand VolumeChangedCommand
        {
            get { return (ICommand)GetValue(VolumeChangedCommandProperty); }
            set { SetValue(VolumeChangedCommandProperty, value); }
        }
        public static readonly DependencyProperty VolumeChangedCommandProperty =
            DependencyProperty.Register("VolumeChangedCommand", typeof(ICommand), typeof(VolumeButton), new PropertyMetadata(null));

        private static void OnVolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as VolumeButton;
            var volume = MathUtil.Clamp((double)e.NewValue, 0, 100);

            self.valueAnimating = true;
            self.Slider.Value = volume / 10;
        }
        #endregion
        public VolumeButton()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!valueAnimating)
            {
                var volume = (int)(e.NewValue * 10);
                VolumeChangedCommand.TryExecute(volume);

            }
            valueAnimating = false;
        }
    }
}
