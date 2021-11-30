using AnimeTime.WPF.Views.ExtendedControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AnimeTime.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        #region Dependency Properties
        public double VerticalContentOffset
        {
            get { return (double)GetValue(VerticalContentOffsetProperty); }
            set { SetValue(VerticalContentOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerticalContentOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalContentOffsetProperty =
            DependencyProperty.Register("VerticalContentOffset", typeof(double), typeof(DetailsView), new PropertyMetadata(0.0));


        public bool WallpaperPassedViewport
        {
            get { return (bool)GetValue(WallpaperPassedViewportProperty); }
            set { SetValue(WallpaperPassedViewportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WallpaperPassedViewport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WallpaperPassedViewportProperty =
            DependencyProperty.Register("WallpaperPassedViewport", typeof(bool), typeof(DetailsView), new PropertyMetadata(false));
        #endregion

        private StackPanel ContentContainer;
        private DefaultScrollViewer DefaultScrollViewer;

        public DetailsView()
        {
            InitializeComponent();
            this.Loaded += DetailsView_Loaded;
        }

        private void DetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTemplateVariables();
            DefaultScrollViewer.ScrollChanged += DefaultScrollViewer_ScrollChanged;
        }

        private void InitializeTemplateVariables()
        {
            ContentContainer = this.Template.FindName("ContentContainer", this) as StackPanel;
            DefaultScrollViewer = this.Template.FindName("DefaultScrollViewer", this) as DefaultScrollViewer;
        }

        private void DefaultScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var point = ContentContainer.TranslatePoint(new Point(), DefaultScrollViewer);
            if (point.Y <= 0)
                WallpaperPassedViewport = true;
            else
                WallpaperPassedViewport = false;
        }
    }
}
