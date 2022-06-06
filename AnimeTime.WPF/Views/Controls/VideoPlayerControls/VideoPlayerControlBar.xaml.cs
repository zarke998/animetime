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
    /// Interaction logic for VideoPlayerControls.xaml
    /// </summary>
    public partial class VideoPlayerControlBar : UserControl
    {
        public int Position { get { return ProgressBar.CurrentTimeSpan.Seconds; } set { ProgressBar.UpdateProgress(value); } }
        #region Dependency Properties
        public int CurrentTime
        {
            get { return (int)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(int), typeof(VideoPlayerControlBar), new PropertyMetadata(0));


        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(int), typeof(VideoPlayerControlBar), new PropertyMetadata(0));


        public ICommand SeekCommand
        {
            get { return (ICommand)GetValue(SeekCommandProperty); }
            set { SetValue(SeekCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeekCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeekCommandProperty =
            DependencyProperty.Register("SeekCommand", typeof(ICommand), typeof(VideoPlayerControlBar), new PropertyMetadata(null));



        public ICommand PlayToggleCommand
        {
            get { return (ICommand)GetValue(PlayToggleCommandProperty); }
            set { SetValue(PlayToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayToggleCommandProperty =
            DependencyProperty.Register("PlayToggleCommand", typeof(ICommand), typeof(VideoPlayerControlBar), new PropertyMetadata(null));



        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(VideoPlayerControlBar), new PropertyMetadata(false));




        public ICommand FullscreenToggleCommand
        {
            get { return (ICommand)GetValue(FullscreenToggleCommandProperty); }
            set { SetValue(FullscreenToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FullscreenToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FullscreenToggleCommandProperty =
            DependencyProperty.Register("FullscreenToggleCommand", typeof(ICommand), typeof(VideoPlayerControlBar), new PropertyMetadata(null));



        public bool IsFullscreen
        {
            get { return (bool)GetValue(IsFullscreenProperty); }
            set { SetValue(IsFullscreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFullscreen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFullscreenProperty =
            DependencyProperty.Register("IsFullscreen", typeof(bool), typeof(VideoPlayerControlBar), new PropertyMetadata(false));



        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Volume.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(double), typeof(VideoPlayerControlBar), new PropertyMetadata(0.0));



        public ICommand VolumeChangedCommand
        {
            get { return (ICommand)GetValue(VolumeChangedCommandProperty); }
            set { SetValue(VolumeChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VolumeChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VolumeChangedCommandProperty =
            DependencyProperty.Register("VolumeChangedCommand", typeof(ICommand), typeof(VideoPlayerControlBar), new PropertyMetadata(null));


        #endregion
        public VideoPlayerControlBar()
        {
            InitializeComponent();
        }
    }
}
