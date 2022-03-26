using AnimeTime.WPF.Commands;
using System;
using System.Collections;
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

        #region Properties
        private int TabCount => Convert.ToInt32(Math.Ceiling(NumberOfEpisodes / (MAX_EPISODES_PER_TAB * 1.0)));

        public int NumberOfEpisodes => Items.Cast<object>().Count();

        #endregion

        #region Dependency Properties

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(Episodes), new PropertyMetadata(new List<object>() , OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            var episodes = GetEpisodesFromItems();
            for (int i = episodeRange.StartEpisode; i <= episodeRange.EndEpisode; i++)
            {
                var episode = episodes.First(e => e.EpNum == i);
                var button = CreateEpisodeButton(episode.EpNum, episode.Value);
                EpisodesContainer.Children.Add(button);
            }
            EpisodesContainer.BeginAnimation(FlexboxPanel.OpacityProperty, FadeInAnimation(new Duration(TimeSpan.FromMilliseconds(400))));
        }
        private Button CreateEpisodeButton(int epNum, object value)
        {
            var button = new Button() { Content = epNum, Tag = value };
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

        private List<Episode> GetEpisodesFromItems()
        {
            var result = new List<Episode>();

            var items = (IEnumerable<object>)Items;
            foreach (var item in items)
            {
                var episode = new Episode();
                var props = item.GetType().GetProperties();

                episode.EpNum = Convert.ToInt32(props.First(p => p.Name == "EpNum").GetValue(item));
                episode.Value = Convert.ToInt32(props.First(p => p.Name == "Id").GetValue(item));
                result.Add(episode);
            }

            return result;
        }
    }

    class Episode
    {
        public int EpNum { get; set; }
        public object Value { get; set; }
    }
    class EpisodeRange
    {
        public int StartEpisode { get; set; }
        public int EndEpisode { get; set; }
    }
}
