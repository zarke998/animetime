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
using System.Linq;
using System.Windows.Media.Animation;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for TabsWithSlider.xaml
    /// </summary>
    public partial class TabsWithSlider : UserControl
    {

        #region Dependency Properties
        public IEnumerable<string> Items
        {
            get { return (IEnumerable<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(TabsWithSlider), new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var slider = (TabsWithSlider)obj;
            slider.InvalidateTabs();
        }
        #endregion

        private Button _activeTab;
        public string ActiveTab
        {
            get
            {
                return _activeTab.Content.ToString();
            }
        }

        public TabsWithSlider()
        {
            InitializeComponent();
            this.Loaded += TabsWithSlider_Loaded;
        }

        #region Events
        private void TabsWithSlider_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeSliderPosition();
        }

        #endregion

        #region Slider
        private void InitializeSliderPosition()
        {
            Slider.Width = SliderWidthFromTab(TabsContainer.Children[0] as Button);
        }
        private void AnimateSlider(int targetTabIndex, int duration)
        {
            var targetTab = TabsContainer.Children[targetTabIndex] as Button;

            ThicknessAnimation sliderPositionAnimation = SliderPositionAnimation(targetTabIndex);
            DoubleAnimation sliderWidthAnimation = SliderWidthAnimation(targetTab);

            var storyboard = new Storyboard();
            storyboard.Children.Add(sliderPositionAnimation);
            storyboard.Children.Add(sliderWidthAnimation);

            storyboard.Begin(Slider);
        }

        private DoubleAnimation SliderWidthAnimation(Button targetTab)
        {
            var sliderWidthAnimation = new DoubleAnimation(Slider.ActualWidth, SliderWidthFromTab(targetTab), new Duration(TimeSpan.FromMilliseconds(500)));
            Storyboard.SetTargetName(sliderWidthAnimation, Slider.Name);
            Storyboard.SetTargetProperty(sliderWidthAnimation, new PropertyPath(Rectangle.WidthProperty));
            return sliderWidthAnimation;
        }
        private ThicknessAnimation SliderPositionAnimation(int targetTabIndex)
        {
            var slideTo = Slider.Margin;
            slideTo.Left = TabPositionX(targetTabIndex);
            var sliderMarginAnimation = new ThicknessAnimation(Slider.Margin, slideTo, new Duration(TimeSpan.FromMilliseconds(500)));
            sliderMarginAnimation.EasingFunction = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            Storyboard.SetTargetName(sliderMarginAnimation, Slider.Name);
            Storyboard.SetTargetProperty(sliderMarginAnimation, new PropertyPath(Rectangle.MarginProperty));
            return sliderMarginAnimation;
        }
        #endregion

        #region Tabs
        private double SliderWidthFromTab(Button tab)
        {
            return tab.ActualWidth + tab.Margin.Right / 2;
        }
        private double TabPositionX(int tabIndex)
        {
            if (tabIndex > TabsContainer.Children.Count - 1)
            {
                throw new ArgumentOutOfRangeException("Index is out of range");
            }

            double x = 0;
            for (int i = 0; i < tabIndex; i++)
            {
                var button = TabsContainer.Children[i] as Button;
                x += button.ActualWidth + button.Margin.Right;
            }

            return x;
        }

        private void InvalidateTabs()
        {
            foreach (var item in Items)
            {
                var button = CreateTab(item);
                TabsContainer.Children.Add(button);
            }
        }
        private Button CreateTab(string displayName)
        {
            var button = new Button();

            button.Style = this.FindResource("NavTab") as Style;
            button.Content = displayName;
            button.Click += Tab_Click;

            return button;
        }

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            var tab = (Button)sender;
            if (_activeTab == tab)
            {
                return;
            }

            _activeTab = tab;
            AnimateSlider(TabsContainer.Children.IndexOf(tab), 500);
        }
        #endregion

    }
}
