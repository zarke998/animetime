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
    /// Interaction logic for PlayButton.xaml
    /// </summary>
    public partial class PlayButton : Button
    {
        #region Dependency Properties

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(PlayButton), new PropertyMetadata(false));



        public ICommand PlayToggleCommand
        {
            get { return (ICommand)GetValue(PlayToggleCommandProperty); }
            set { SetValue(PlayToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayToggleCommandProperty =
            DependencyProperty.Register("PlayToggleCommand", typeof(ICommand), typeof(PlayButton), new PropertyMetadata(null));


        #endregion


        public PlayButton()
        {
            InitializeComponent();
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            IsPlaying = !IsPlaying;

            PlayToggleCommand.TryExecute(IsPlaying);
        }
    }
}
