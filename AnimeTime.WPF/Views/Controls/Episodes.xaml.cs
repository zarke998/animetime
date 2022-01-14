using AnimeTime.WPF.Commands;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for Episodes.xaml
    /// </summary>
    public partial class Episodes : UserControl
    {
        public const int MAX_EPISODES_PER_TAB = 50;

        private int TabCount => Convert.ToInt32(Math.Ceiling(NumberOfEpisodes / (MAX_EPISODES_PER_TAB * 1.0)));

        #region Dependency Properties
        public int NumberOfEpisodes
        {
            get { return (int)GetValue(NumberOfEpisodesProperty); }
            set { SetValue(NumberOfEpisodesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberOfEpisodes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfEpisodesProperty =
            DependencyProperty.Register("NumberOfEpisodes", typeof(int), typeof(Episodes), new PropertyMetadata(0, OnEpisodeCountChanged));

        private static void OnEpisodeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as Episodes;
            self.InvalidateTabs();
        }



        public ICommand EpisodeSelectedCommand
        {
            get { return (ICommand)GetValue(EpisodeSelectedCommandProperty); }
            set { SetValue(EpisodeSelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EpisodeSelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EpisodeSelectedCommandProperty =
            DependencyProperty.Register("EpisodeSelectedCommand", typeof(ICommand), typeof(Episodes), new PropertyMetadata(null));
        #endregion

        public Episodes()
        {
            InitializeComponent();
        }

        #region TabSlider
        private void InvalidateTabs()
        {
            Tabs.Items = CreateTabs();
            Tabs.Command = new DelegateCommand(LoadEpisodes);
        }
        private Dictionary<string, object> CreateTabs()
        {
            var tabs = new Dictionary<string, object>();

            if (TabCount == 1)
            {
                var episodeRange = new EpisodeRange()
                {
                    StartEpisode = 1,
                    EndEpisode = NumberOfEpisodes
                };
                var displayName = $"{episodeRange.StartEpisode} - {episodeRange.EndEpisode}";

                tabs.Add(displayName, episodeRange);

                return tabs;
            }
            for (int i = 0; i < TabCount; i++)
            {
                var episodeRange = new EpisodeRange()
                {
                    StartEpisode = MAX_EPISODES_PER_TAB * i + 1,
                    EndEpisode = MAX_EPISODES_PER_TAB * (i + 1)
                };
                var displayName = $"{episodeRange.StartEpisode} - {episodeRange.EndEpisode}";

                tabs.Add(displayName, episodeRange);
            }

            return tabs;
        }
        #endregion

        #region EpisodesContainer
        private void LoadEpisodes(object param)
        {
            EpisodesContainer.Children.Clear();

            EpisodesContainer.BeginAnimation(FlexboxPanel.OpacityProperty, null);
            EpisodesContainer.Opacity = 0;

            var episodeRange = param as EpisodeRange;
            for (int i = episodeRange.StartEpisode; i <= episodeRange.EndEpisode; i++)
            {
                var button = CreateEpisodeButton(i);
                EpisodesContainer.Children.Add(button);
            }
            EpisodesContainer.BeginAnimation(FlexboxPanel.OpacityProperty, FadeInAnimation(new Duration(TimeSpan.FromMilliseconds(400))));
        }
        private Button CreateEpisodeButton(int epNum)
        {
            var button = new Button() { Content = epNum, Tag = epNum };
            button.Style = this.FindResource("SingleEpisodeButton") as Style;
            button.Margin = new Thickness(0, 0, 0, 10);
            button.Click += EpisodeButton_Click;
            return button;
        }

        private void EpisodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EpisodeSelectedCommand != null && EpisodeSelectedCommand.CanExecute(null))
                EpisodeSelectedCommand.Execute((sender as Button).Tag);
        }
        #endregion

        #region Animations
        private DoubleAnimation FadeInAnimation(Duration duration)
        {
            var opacityAnimation = new DoubleAnimation();
            opacityAnimation.To = 1.0;
            opacityAnimation.Duration = duration;

            return opacityAnimation;
        }
        #endregion
    }

    class EpisodeRange
    {
        public int StartEpisode { get; set; }
        public int EndEpisode { get; set; }
    }
}
