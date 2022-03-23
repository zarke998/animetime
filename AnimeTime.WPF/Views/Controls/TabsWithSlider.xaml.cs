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
        public Dictionary<string, object> Items
        {
            get { return (Dictionary<string, object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public double TabSize
        {
            get { return (double)GetValue(TabSizeProperty); }
            set { SetValue(TabSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TabSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabSizeProperty =
            DependencyProperty.Register("TabSize", typeof(double), typeof(TabsWithSlider), new PropertyMetadata(22.0));

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TabsWithSlider), new PropertyMetadata(OnCommandChanged));

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(Dictionary<string, object>), typeof(TabsWithSlider), new PropertyMetadata(new Dictionary<string, object>(), OnItemsChanged));

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var slider = (TabsWithSlider)obj;

            var oldCmd = args.OldValue as ICommand;
            var newCmd = args.NewValue as ICommand;

            if (newCmd == null)
            {
                oldCmd.CanExecuteChanged -= slider.Command_CanExecuteChanged;
            }

            else if (oldCmd == null)
            {
                newCmd.CanExecuteChanged += slider.Command_CanExecuteChanged;
            }
            else
            {
                oldCmd.CanExecuteChanged -= slider.Command_CanExecuteChanged;
                newCmd.CanExecuteChanged += slider.Command_CanExecuteChanged;
            }
        }

        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var slider = (TabsWithSlider)obj;
            slider.InvalidateTabs();
            slider.FirstTabFire();
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
            FirstTabFire();
        }

        #endregion

        #region Slider
        private void InitializeSliderPosition()
        {
            if (TabsContainer.Children.Count == 0) return;

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
                var button = CreateTab(item.Key, item.Value);
                TabsContainer.Children.Add(button);
            }
        }
        private Button CreateTab(string displayName, object value)
        {
            var button = new Button();

            button.Style = this.FindResource("NavTab") as Style;

            Binding fontSizeBinding = new Binding();
            fontSizeBinding.Source = this;
            fontSizeBinding.Path = new PropertyPath(TabsWithSlider.TabSizeProperty);

            button.SetBinding(Button.FontSizeProperty, fontSizeBinding);
            button.Content = displayName;
            button.Tag = value;
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

            ExecuteCommand(tab.Tag);

            AnimateSlider(TabsContainer.Children.IndexOf(tab), 500);
        }
        private void FirstTabFire()
        {
            var firstTab = TabsContainer.Children.Cast<Button>().FirstOrDefault();
            if (firstTab == null) return;

            _activeTab = firstTab;
            ExecuteCommand(firstTab.Tag);
        }

        private void DisableTabs()
        {
            foreach (Button tab in TabsContainer.Children)
            {
                tab.IsEnabled = false;
            }
        }
        private void EnableTabs()
        {
            foreach (Button tab in TabsContainer.Children)
            {
                tab.IsEnabled = true;
            }
        }
        #endregion

        private void ExecuteCommand(object param)
        {
            if (Command != null && Command.CanExecute(null))
            {
                Command.Execute(param);
            }
        }
        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if(Command.CanExecute(null))
            {
                EnableTabs();
            }
            else
            {
                DisableTabs();
            }
        }
    }

    public class TabItem
    {
        public string DisplayName { get; set; }
        public object Value { get; set; }
    }
}
