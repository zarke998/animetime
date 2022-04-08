using AnimeTime.Utilities.MathHelpers;
using AnimeTime.WPF.Utility;
using AnimeTime.WPF.Views.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ProgressBar : UserControlBase
    {
        private Thumb _thumb;
        private Border _trackBackground;
        private TimeSpan _durationTime;
        private TimeSpan _currentTime;

        public TimeSpan CurrentTime { get => _currentTime; set { _currentTime = value; OnPropertyChanged(); } }
        public TimeSpan DurationTime { get => _durationTime; set { _durationTime = value; OnPropertyChanged(); } }

        #region Depdendency Properties

        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(int), typeof(ProgressBar), new PropertyMetadata(0, DurationChanged));

        public ICommand ValueChangedCommand
        {
            get { return (ICommand)GetValue(ValueChangedCommandProperty); }
            set { SetValue(ValueChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueChangedCommandProperty =
            DependencyProperty.Register("ValueChangedCommand", typeof(ICommand), typeof(ProgressBar), new PropertyMetadata(null));


        private static void DurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ProgressBar;

            self.DurationTime = TimeSpan.FromSeconds((int)e.NewValue);
        }
        #endregion

        public ProgressBar()
        {
            InitializeComponent();
            this.Loaded += ProgressBar_Loaded;
            Slider.ValueChanged += Slider_ValueChanged;
        }
        public void SetPosition(int position)
        {
            CurrentTime = TimeSpan.FromSeconds(position);
            UpdateText();

            var sliderPosition = ((position * 1.0) / Duration) * 10;
            Slider.Value = sliderPosition;
        }
        #region Events
        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            _thumb = Slider.Template.FindName("Thumb", Slider) as Thumb;
            _trackBackground = Slider.Template.FindName("TrackBackground", Slider) as Border;
            SetThumbColorFromGradientOffset(0);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CurrentTime = TimeSpan.FromSeconds((int)(Duration * (e.NewValue / 10)));
            UpdateText();
            SetThumbColorFromGradientOffset(e.NewValue / 10);
        }
        private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedTime = (int)(Duration * (Slider.Value / 10));
            ValueChangedCommand.TryExecute(selectedTime);
        }
        #endregion
        private void SetThumbColorFromGradientOffset(double offset)
        {
            offset = MathUtil.Clamp(offset, 0, 1);

            var gradient = _trackBackground.Background as LinearGradientBrush;
            UpdateThumbColor(GradientExtensions.GetRelativeColor(gradient.GradientStops, offset));
        }
        private void UpdateThumbColor(Color color)
        {
            _thumb.Background = new SolidColorBrush(color);
        }

        private void UpdateText()
        {
            TimeDisplay.Text = String.Format(@"{0:mm':'ss} \\ {1:mm':'ss}", CurrentTime, DurationTime);
        }
    }
}
