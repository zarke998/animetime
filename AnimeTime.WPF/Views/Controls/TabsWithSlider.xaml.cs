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
using AnimeTime.WPF.Converters;
using System.Collections;
using AnimeTime.Utilities.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for TabsWithSlider.xaml
    /// </summary>
    public partial class TabsWithSlider : UserControl
    {
        #region Dependency Properties
        public double TabSize
        {
            get { return (double)GetValue(TabSizeProperty); }
            set { SetValue(TabSizeProperty, value); }
        }
        public static readonly DependencyProperty TabSizeProperty =
            DependencyProperty.Register("TabSize", typeof(double), typeof(TabsWithSlider), new PropertyMetadata(22.0));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TabsWithSlider), new PropertyMetadata(OnCommandChanged));

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(TabsWithSlider), new PropertyMetadata(null, OnItemsChanged));

        public string ActiveTab
        {
            get { return (string)GetValue(ActiveTabProperty); }
            set { SetValue(ActiveTabProperty, value); }
        }
        public static readonly DependencyProperty ActiveTabProperty =
            DependencyProperty.Register("ActiveTab", typeof(string), typeof(TabsWithSlider), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnActiveTabChanged));

        #region DP events
        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var slider = (TabsWithSlider)obj;

            var oldArg = args.OldValue as INotifyCollectionChanged;
            var newArg = args.NewValue as INotifyCollectionChanged;

            if (oldArg != null)
                oldArg.CollectionChanged -= slider.OnItemsCollectionChanged;

            if (newArg != null)
            {
                slider._items = (newArg as IEnumerable).MapToList<TabItem>();
                newArg.CollectionChanged += slider.OnItemsCollectionChanged;

                slider.InvalidateTabs();
                slider.FirstTabFire();
            }
            
            if(newArg == null || (args.NewValue as ICollection).Count == 0)
            {
                slider.ClearTabs();
                slider.ResetSlider();
            }

        }
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
        private static void OnActiveTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as TabsWithSlider;

            if (e.NewValue.ToString() == String.Empty)
            {
                slider.ResetSlider();
            }
            else
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    slider.SetSliderPosition(e.NewValue.ToString());
                }, System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
        }
        #endregion

        #endregion

        #region Members
        private List<TabItem> _items = new List<TabItem>();
        private Button _activeTabButton;
        #endregion

        #region Properties
        public List<Button> Tabs => TabsContainer.Children.Cast<Button>().ToList();
        #endregion

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
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _items = (sender as IEnumerable).MapToList<TabItem>();
            InvalidateTabs();
        }
        #endregion

        #region Slider
        private void InitializeSliderPosition()
        {
            if (TabsContainer.Children.Count == 0) return;

            var firstTab = TabsContainer.Children[0] as Button;
            var binding = new Binding("ActualWidth")
            {
                Source = firstTab,
                Converter = new AdditionConverter(),
                ConverterParameter = firstTab.Margin.Right / 2
            };
            Slider.SetBinding(Rectangle.WidthProperty, binding);
        }
        private void SetSliderPosition(string tabName)
        {
            var targetTab = TabsContainer.Children.Cast<Button>().FirstOrDefault(x => x.Content.ToString() == tabName);
            if (targetTab != null)
                targetTab.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
        private void ResetSlider()
        {
            Slider.BeginAnimation(Rectangle.WidthProperty, null);
            Slider.Width = 0;

            var margin = Slider.Margin;
            margin.Left = 0;

            Slider.Margin = margin;
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
        private void InvalidateTabs()
        {
            ClearTabs();
            foreach (var item in _items)
            {
                var button = CreateTab(item.Title, item.Value);
                TabsContainer.Children.Add(button);
            }
        }
        private void ClearTabs()
        {
            TabsContainer.Children.Clear();
        }
        private void FirstTabFire()
        {
            var firstTab = TabsContainer.Children.Cast<Button>().FirstOrDefault();
            if (firstTab == null) return;

            _activeTabButton = firstTab;

            InitializeSliderPosition();
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

        #region Tab item
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
        private double SliderWidthFromTab(Button tab)
        {
            if (tab == null) return 0;
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
        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            var tab = (Button)sender;
            if (_activeTabButton == tab)
            {
                return;
            }
            _activeTabButton = tab;

            ExecuteCommand(tab.Tag);

            AnimateSlider(TabsContainer.Children.IndexOf(tab), 500);
        }
        #endregion
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
            if (Command.CanExecute(null))
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
        public string Title { get; set; }
        public object Value { get; set; }
    }
}
