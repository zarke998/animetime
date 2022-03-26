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

        // Using a DependencyProperty as the backing store for Sources.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourcesProperty =
            DependencyProperty.Register("Sources", typeof(IEnumerable<string>), typeof(PlayerWindow), new PropertyMetadata(new List<string>(), OnSourcesChanged));

        private static void OnSourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as PlayerWindow;
            if (e.NewValue != null)
                self.LoadEpisode();
        }

        #endregion

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public PlayerWindow()
        {
            InitializeComponent();
            this.Loaded += PlayerWindow_Loaded;
            this.Closed += PlayerWindow_Closed;


        }

        private void BindSourcesToDataContext()
        {
            var binding = new Binding();
            binding.Source = this.DataContext;
            binding.Path = new PropertyPath("Sources");

            this.SetBinding(SourcesProperty, binding);
        }

        private async Task InitializeVLC()
        {
            await Task.Run(() =>
            {
                LibVLCSharp.Shared.Core.Initialize();
                _libVLC = new LibVLC();
            });
        }
        private void ShutdownVLC()
        {
            VlcVideoView.MediaPlayer.Stop();
            VlcVideoView.MediaPlayer.Dispose();
            _libVLC.Dispose();
        }

        private void PlayerWindow_Closed(object sender, EventArgs e)
        {
            ShutdownVLC();
        }

        private async void PlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeVLC();
            BindSourcesToDataContext();

            _mediaPlayer = new MediaPlayer(_libVLC);
            VlcVideoView.MediaPlayer = _mediaPlayer;

            
        }

        private void LoadEpisode()
        {
            var sourceUrl = Sources.LastOrDefault();
            if (sourceUrl == null) return;

            _mediaPlayer.Play(new Media(_libVLC, new Uri(sourceUrl)));
        }
    }
}
