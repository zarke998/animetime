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
        private LibVLC _libVLC;
        public PlayerWindow()
        {
            InitializeComponent();
            InitializeVLC();
            this.Loaded += PlayerWindow_Loaded;
            this.Closed += PlayerWindow_Closed;
        }

        private void InitializeVLC()
        {
            LibVLCSharp.Shared.Core.Initialize();
            _libVLC = new LibVLC();
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

        private void PlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var mediaPlayer = new MediaPlayer(_libVLC);
            VlcVideoView.MediaPlayer = mediaPlayer;

            mediaPlayer.Play(new Media(_libVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4")));
        }
    }
}
