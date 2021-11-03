using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AnimeTime.Utilities.Collections;
using AnimeTime.WPF.Converters;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for Carousel.xaml
    /// </summary>
    public partial class Carousel : UserControl
    {

        #region Dependency Properties
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(Carousel), new PropertyMetadata(new ObservableCollection<object>()));
        #endregion

        #region Constants
        // Item dimensions
        public double CenterItemWidth => 400;
        public double CenterItemHeight => Container.Height;

        public double ItemWidth => 250;
        public double ItemHeight => Container.Height - 50;

        // Carousel buttons
        public const int CAROUSEL_BUTTON_OFFSET = 5;

        //Transition
        private const int TRANSITION_SPEED = 1000;

        #endregion

        private StackPanel[] _carouselItems;

        private int _startIndex;
        private int _endIndex;

        private bool _isAnimating = false;

        public Carousel()
        {
            InitializeComponent();
            this.Loaded += Carousel_Loaded;

            _startIndex = 0;
            _endIndex = 2;
            InitializeData();

            MoveIndexLeft(ref _startIndex, 1);
            MoveIndexRight(ref _endIndex, 1);

            _carouselItems = new StackPanel[5];
        }

        private void InitializeData()
        {
            for (int i = 0; i < 3; i++)
            {
                Items.Add($"Test {i + 1}");
            }
        }
        private void Carousel_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCarouselItems();

            InitializeButtons();
        }

        #region Initialization
        private void InitializeButtons()
        {
            InitializeButtonLeft();
            InitializeButtonRight();
        }
        private void InitializeButtonRight()
        {
            var button = CreateCarouselButton(1, this.FindResource("SlideButtonRight") as Style);

            Canvas.SetRight(button, CAROUSEL_BUTTON_OFFSET);
            Canvas.SetTop(button, Container.Height / 2 - button.Height / 2);

            button.Click += ToLeft_Click;
            Container.Children.Add(button);
        }
        private void InitializeButtonLeft()
        {
            var button = CreateCarouselButton(-1, this.FindResource("SlideButtonLeft") as Style);

            Canvas.SetLeft(button, CAROUSEL_BUTTON_OFFSET);
            Canvas.SetTop(button, Container.Height / 2 - button.Height / 2);

            button.Click += ToRight_Click;
            Container.Children.Add(button);
        }
        private Button CreateCarouselButton(int slideIncrement, Style style = null)
        {
            Button button = new Button()
            {
                Width = 25,
                Height = 25,
                Tag = slideIncrement
            };

            if (style != null)
                button.Style = style;

            Canvas.SetZIndex(button, 999);

            return button;
        }

        private void InitializeCarouselItems()
        {
            var itemIndexes = GetItemIndexes();

            for (int i = -1; i <= 3; i++)
            {
                var item = CreateItem(Items[itemIndexes[i + 1]].ToString());
                if (i == 1)
                {
                    item.Width = CenterItemWidth;
                    item.Height = CenterItemHeight;
                }
                else
                {
                    item.Width = ItemWidth;
                    item.Height = ItemHeight;
                }

                SetItemPosition(ref item, i);
                Container.Children.Add(item);
                _carouselItems[i + 1] = item;
            }
        }
        #endregion

        private void SetItemPosition(ref StackPanel item, int position)
        {
            var offset = 0;
            var itemSpacing = 50;
            var fullPositionOffset = item.Width + offset + itemSpacing;
            switch (position)
            {
                case -1:
                    Canvas.SetLeft(item, -offset - fullPositionOffset);
                    Canvas.SetTop(item, Container.Height / 2 - item.Height / 2);
                    break;
                case 0:
                    Canvas.SetLeft(item, -offset);
                    Canvas.SetTop(item, Container.Height / 2 - item.Height / 2);
                    break;
                case 1:
                    Canvas.SetLeft(item, Container.Width / 2 - item.Width / 2);
                    Canvas.SetTop(item, 0);
                    break;
                case 2:
                    Canvas.SetLeft(item, Container.Width - item.Width + offset);
                    Canvas.SetTop(item, Container.Height / 2 - item.Height / 2);
                    break;
                case 3:
                    Canvas.SetLeft(item, Container.Width - item.Width + fullPositionOffset + offset);
                    Canvas.SetTop(item, Container.Height / 2 - item.Height / 2);
                    break;
                default:
                    throw new ArgumentException("Valid positions are -1 to 3 (inclusive).");
            }
        }
        private StackPanel CreateItem(string title)
        {
            var root = new StackPanel();
            root.Orientation = Orientation.Vertical;

            var carouselItem = new CarouselItem();

            Binding heightBinding = new Binding(nameof(root.Height));
            heightBinding.Source = root;
            heightBinding.Converter = new AdditionConverter();
            heightBinding.ConverterParameter = -25;

            carouselItem.SetBinding(CarouselItem.HeightProperty, heightBinding);

            var textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.Text = title;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.FontSize = 18;
            textBlock.Margin = new Thickness(0, 5, 0, 0);

            root.Children.Add(carouselItem);
            root.Children.Add(textBlock);

            return root;
        }
        private Storyboard CreateItemTransition(StackPanel toAnimate, StackPanel target, Duration duration)
        {
            DoubleAnimation canvasLeftAnimation = new DoubleAnimation(Canvas.GetLeft(target), duration);
            DoubleAnimation canvasTopAnimation = new DoubleAnimation(Canvas.GetTop(target), duration);
            DoubleAnimation widthAnimation = new DoubleAnimation(target.Width, duration);
            DoubleAnimation heightAnimation = new DoubleAnimation(target.Height, duration);

            var easingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

            canvasLeftAnimation.EasingFunction = easingFunction;
            canvasTopAnimation.EasingFunction = easingFunction;
            widthAnimation.EasingFunction = easingFunction;
            heightAnimation.EasingFunction = easingFunction;

            var storyboard = new Storyboard();
            storyboard.Children.Add(canvasLeftAnimation);
            storyboard.Children.Add(canvasTopAnimation);
            storyboard.Children.Add(widthAnimation);
            storyboard.Children.Add(heightAnimation);

            Storyboard.SetTarget(canvasLeftAnimation, toAnimate);
            Storyboard.SetTargetProperty(canvasLeftAnimation, new PropertyPath(Canvas.LeftProperty));

            Storyboard.SetTarget(canvasTopAnimation, toAnimate);
            Storyboard.SetTargetProperty(canvasTopAnimation, new PropertyPath(Canvas.TopProperty));

            Storyboard.SetTarget(widthAnimation, toAnimate);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Rectangle.WidthProperty));

            Storyboard.SetTarget(heightAnimation, toAnimate);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Rectangle.HeightProperty));

            return storyboard;
        }

        private List<int> GetItemIndexes()
        {
            var result = new List<int>();

            var indexLeft = _startIndex;
            var indexRight = _endIndex;

            while (result.Count < 5)
            {
                result.Add(_startIndex);
                MoveIndexRight(ref _startIndex, 1);
            }
            _startIndex = indexLeft;

            return result;
        }

        #region Index moving
        private void MoveIndex(ref int index, int by)
        {
            if (by < 0)
                MoveIndexLeft(ref index, Math.Abs(by));
            else if (by > 0)
                MoveIndexRight(ref index, by);
        }
        private int MoveIndexLeft(ref int index, int forAmount)
        {
            forAmount = forAmount % Items.Count;

            index -= forAmount;
            if (index < 0)
                index = Items.Count + index;

            return index;
        }
        private int MoveIndexRight(ref int index, int forAmount)
        {
            forAmount = forAmount % Items.Count;

            index += forAmount;
            if (index >= Items.Count)
                index = 0 + (index - Items.Count);

            return index;
        }
        #endregion

        #region Arrow buttons
        private void ToRight_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating) return;

            var transitions = GetTransitionsToRight(TRANSITION_SPEED);

            _isAnimating = true;
            foreach (var transition in transitions)
            {
                transition.Completed += Transition_Completed;
                transition.Begin();
            }

            RemoveLastItem();

            _carouselItems.ShiftRight();
            MoveIndexLeft(ref _startIndex, 1);

            SpawnItemLeft();
        }
        private void ToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating) return;

            var transitions = GetTransitionsToLeft(TRANSITION_SPEED);

            _isAnimating = true;
            foreach (var transition in transitions)
            {
                transition.Completed += Transition_Completed;
                transition.Begin();
            }

            RemoveFirstItem();

            _carouselItems.ShiftLeft();
            MoveIndexRight(ref _startIndex, 1);

            SpawnItemRight();
        }
        #endregion

        private void SpawnItemRight()
        {
            var startIndex = _startIndex;

            var endIndex = MoveIndexRight(ref _startIndex, 5);
            _startIndex = startIndex;

            var item = CreateItem(Items[endIndex].ToString());
            item.Width = ItemWidth;
            item.Height = ItemHeight;

            SetItemPosition(ref item, 3);
            Container.Children.Add(item);
            _carouselItems[4] = item;
        }
        private void SpawnItemLeft()
        {
            var newFirst = CreateItem(Items[_startIndex].ToString());
            newFirst.Width = ItemWidth;
            newFirst.Height = ItemHeight;

            SetItemPosition(ref newFirst, -1);
            Container.Children.Add(newFirst);
            _carouselItems[0] = newFirst;
        }
        private void RemoveLastItem()
        {
            var last = _carouselItems[4];
            Container.Children.Remove(last);
        }        
        private void RemoveFirstItem()
        {
            var first = _carouselItems[0];
            Container.Children.Remove(first);
        }

        private List<Storyboard> GetTransitionsToRight(int duration)
        {
            var transitions = new List<Storyboard>();
            for (int i = 0; i < _carouselItems.Length - 1; i++)
            {
                transitions.Add(CreateItemTransition(_carouselItems[i], _carouselItems[i + 1], new Duration(TimeSpan.FromMilliseconds(duration))));
            }

            return transitions;
        }
        private List<Storyboard> GetTransitionsToLeft(int duration)
        {
            var transitions = new List<Storyboard>();
            for (int i = _carouselItems.Length - 1; i > 0; i--)
            {
                transitions.Add(CreateItemTransition(_carouselItems[i], _carouselItems[i - 1], new Duration(TimeSpan.FromMilliseconds(duration))));
            }

            return transitions;
        }
        private void Transition_Completed(object sender, EventArgs e)
        {
            _isAnimating = false;
        }
    }
}
