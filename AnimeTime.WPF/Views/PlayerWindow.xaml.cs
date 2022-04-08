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

namespace AnimeTime.WPF.Views
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : WindowBase
    {

        #region Dependency Properties
        public IEnumerable<string> Sources
        {
            get { return (IEnumerable<string>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }

        private const string LIBVLC_CANNOT_OPEN_SOURCE_ERROR = "Your input can't be opened";

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

        public PlayerWindow()
        {
            InitializeComponent();
            this.Loaded += PlayerWindow_Loaded;
            this.Closed += PlayerWindow_Closed;
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
            VlcVideoView.MediaPlayer = _mediaPlayer;

            BindSourcesToDataContext();
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
        #region Events
        private void _libVLC_Log(object sender, LogEventArgs e)
        {
            if(e.FormattedLog.Contains(LIBVLC_CANNOT_OPEN_SOURCE_ERROR))
            {
                if(_videoSources.Count > 0)
                {
                    var source = _videoSources.Pop();
                    _mediaPlayer.Play(new Media(_libVLC, new Uri(source)));
                }
            }
        }
        #endregion
    }
}
