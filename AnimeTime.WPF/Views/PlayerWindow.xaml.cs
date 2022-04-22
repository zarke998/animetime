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
using LibVLCSharp.Shared;
using System.Diagnostics;
using System.Timers;
using AnimeTime.WPF.Commands;

namespace AnimeTime.WPF.Views
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : WindowBase
    {
        private const string LIBVLC_CANNOT_OPEN_SOURCE_ERROR = "Your input can't be opened";

        #region Dependency Properties
        public IEnumerable<string> Sources
        {
            get { return (IEnumerable<string>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Sources.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourcesProperty =
            DependencyProperty.Register("Sources", typeof(IEnumerable<string>), typeof(PlayerWindow), new PropertyMetadata(new List<string>(), OnSourcesChanged));

        private static void OnSourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as PlayerWindow;
            var newValue = e.NewValue as IEnumerable<object>;
            if (newValue != null && newValue.Count() > 0)
                self.LoadSources();
        }

        #endregion

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        private Stack<string> _videoSources;

        private int _currentTime;
        private Timer _progressTimer;
        private ICommand _seekCommand;
        private ICommand _playToggleCommand;

        private ICommand _fullscreenToggleCommand;
        private bool _isPlaying;

        public ICommand SeekCommand { get => _seekCommand; set { _seekCommand = value; OnPropertyChanged(); } }
        public ICommand PlayToggleCommand { get => _playToggleCommand; set { _playToggleCommand = value; OnPropertyChanged(); } }
        public ICommand FullscreenToggleCommand { get => _fullscreenToggleCommand; set { _fullscreenToggleCommand = value; OnPropertyChanged(); } }
        public bool IsPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(); } }


        public PlayerWindow()
        {
            InitializeComponent();
            this.Loaded += PlayerWindow_Loaded;
            this.Closed += PlayerWindow_Closed;

            _progressTimer = new Timer(1000);
            _progressTimer.Elapsed += _progressTimer_Elapsed;

            SeekCommand = new DelegateCommand(Seek);
            PlayToggleCommand = new DelegateCommand(PlayToggle);
            FullscreenToggleCommand = new DelegateCommand(FullscreenToggle);
        }

        #region Events
        private void PlayerWindow_Closed(object sender, EventArgs e)
        {
            ShutdownVLC();
            BindingOperations.ClearBinding(this, SourcesProperty);
        }
        private void PlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeVLC();

            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.LengthChanged += _mediaPlayer_LengthChanged;
            _mediaPlayer.Playing += _mediaPlayer_Playing;
            _mediaPlayer.Paused += _mediaPlayer_Paused;
            _mediaPlayer.Stopped += _mediaPlayer_Stopped;
            _mediaPlayer.Buffering += _mediaPlayer_Buffering;
            _mediaPlayer.TimeChanged += _mediaPlayer_TimeChanged;
            VlcVideoView.MediaPlayer = _mediaPlayer;

            BindSourcesToDataContext();
        }
        private void _libVLC_Log(object sender, LogEventArgs e)
        {
            if (e.FormattedLog.Contains(LIBVLC_CANNOT_OPEN_SOURCE_ERROR))
            {
                if (_videoSources.Count > 0)
                {
                    var source = _videoSources.Pop();
                    _mediaPlayer.Play(new Media(_libVLC, new Uri(source)));
                }
            }
        }
        private void _mediaPlayer_LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            Dispatcher.InvokeAsync(() => ControlBar.Duration = Convert.ToInt32(e.Length / 1000));
        }

        #region MediaPlayer States
        private void _mediaPlayer_Playing(object sender, EventArgs e)
        {
            _progressTimer.Start();
            Dispatcher.InvokeAsync(() => IsPlaying = true);
        }
        private void _mediaPlayer_Stopped(object sender, EventArgs e)
        {
            _progressTimer.Stop();
            _currentTime = 0;
        }
        private void _mediaPlayer_Paused(object sender, EventArgs e)
        {
            _progressTimer.Stop();
            Dispatcher.InvokeAsync(() => ControlBar.IsPlaying = false);
        }
        private void _mediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            _progressTimer.Stop();
        }
        private void _mediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            Dispatcher.InvokeAsync(() => ControlBar.Position = (int)(e.Time / 1000));
        }
        #endregion

        private void _progressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _currentTime++;
            Dispatcher.InvokeAsync(() => ControlBar.Position = _currentTime);
        }
        #endregion
        private void BindSourcesToDataContext()
        {
            var binding = new Binding();
            binding.Source = this.DataContext;
            binding.Path = new PropertyPath("Sources");

            this.SetBinding(SourcesProperty, binding);
        }
        private void InitializeVLC()
        {
            LibVLCSharp.Shared.Core.Initialize();
            _libVLC = new LibVLC();
            _libVLC.Log += _libVLC_Log;
        }

        private void ShutdownVLC()
        {
            VlcVideoView.MediaPlayer.Stop();
            VlcVideoView.MediaPlayer.Dispose();
            _libVLC.Dispose();
        }
        private void LoadSources()
        {
            _videoSources = new Stack<string>(Sources.Reverse());

            _mediaPlayer.Play(new Media(_libVLC, new Uri(_videoSources.Pop())));
        }
        private void Seek(object obj)
        {
            int position = (int)obj;

            _currentTime = position;
            _mediaPlayer.SeekTo(TimeSpan.FromSeconds(position));
        }
        private void PlayToggle(object obj)
        {
            var isPlaying = (bool)obj;
            IsPlaying = isPlaying;

            if (isPlaying)
                _mediaPlayer?.Play();
            else
                _mediaPlayer?.Pause();
        }

        private void FullscreenToggle(object obj)
        {
            var isFullscreen = (bool)obj;
            base.FulllscreenToggle(isFullscreen);
        }
    }
}
