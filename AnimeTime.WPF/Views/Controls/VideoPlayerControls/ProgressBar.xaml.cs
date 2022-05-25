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
        private TimeSpan _durationTimeSpan;
        private TimeSpan _currentTimeSpan;
        private bool _isDraging;

        public TimeSpan CurrentTimeSpan { get => _currentTimeSpan; set { _currentTimeSpan = value; OnPropertyChanged(); } }
        public TimeSpan DurationTimeSpan { get => _durationTimeSpan; set { _durationTimeSpan = value; OnPropertyChanged(); } }

        #region Depdendency Properties

        public int CurrentTime
        {
            get { return (int)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(int), typeof(ProgressBar), new PropertyMetadata(0, CurrentTimeChanged));

        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(int), typeof(ProgressBar), new PropertyMetadata(0, DurationChanged));

        public ICommand SeekCommand
        {
            get { return (ICommand)GetValue(SeekCommandProperty); }
            set { SetValue(SeekCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeekCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeekCommandProperty =
            DependencyProperty.Register("SeekCommand", typeof(ICommand), typeof(ProgressBar), new PropertyMetadata(null));

        private static void CurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ProgressBar;
            self.UpdateProgress((int)e.NewValue);
        }
        private static void DurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ProgressBar;

            self.DurationTimeSpan = TimeSpan.FromSeconds((int)e.NewValue);
        }
        #endregion

        public ProgressBar()
        {
            InitializeComponent();
            this.Loaded += ProgressBar_Loaded;
            Slider.ValueChanged += Slider_ValueChanged;
            Slider.GotMouseCapture += Slider_GotMouseCapture;
            Slider.LostMouseCapture += Slider_LostMouseCapture;
        }

        public void UpdateProgress(int seconds)
        {
            if (_isDraging) return;

            CurrentTimeSpan = TimeSpan.FromSeconds(seconds);
            UpdateText();

            if (Duration > 0)
            {
                var sliderPosition = ((seconds * 1.0) / Duration) * 10;
                Slider.Value = sliderPosition;
            }
        }
        #region Events
        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _thumb = Slider.Template.FindName("Thumb", Slider) as Thumb;
            _trackBackground = Slider.Template.FindName("TrackBackground", Slider) as Border;
            SetThumbColorFromGradientOffset(0);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraging)
            {
                CurrentTimeSpan = TimeSpan.FromSeconds((int)(Duration * (e.NewValue / 10)));
                UpdateText();
            }
            SetThumbColorFromGradientOffset(e.NewValue / 10);
        }
        private void Slider_GotMouseCapture(object sender, MouseEventArgs e)
        {
            _isDraging = true;
        }
        private void Slider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _isDraging = false;

            var selectedTime = (int)(Duration * (Slider.Value / 10));
            SeekCommand.TryExecute(selectedTime);
        }
        #endregion
        private void SetThumbColorFromGradientOffset(double offset)
        {
            if (_trackBackground == null) return;

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
            Debug.WriteLine(CurrentTimeSpan);
            TimeDisplay.Text = String.Format(@"{0:mm':'ss} \ {1:mm':'ss}", CurrentTimeSpan, DurationTimeSpan);
        }

    }
}
