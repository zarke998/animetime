using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for DefaultScrollViewer.xaml
    /// </summary>
    public partial class DefaultScrollViewer : ScrollViewer
    {
        private DispatcherTimer _timer;

        #region Dependency Properties

        public bool IsScrolling
        {
            get { return (bool)GetValue(IsScrollingProperty); }
            set { SetValue(IsScrollingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsScrolling.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsScrollingProperty =
            DependencyProperty.Register("IsScrolling", typeof(bool), typeof(DefaultScrollViewer), new PropertyMetadata(false));

        #endregion

        public DefaultScrollViewer()
        {
            InitializeComponent();
            this.ScrollChanged += DefaultScrollViewer_ScrollChanged;

            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(100);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            IsScrolling = false;
            _timer.Stop();
        }

        private void DefaultScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            IsScrolling = true;
            _timer.Start();            
        }
    }
}
