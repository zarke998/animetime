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
        #endregion

        public DetailsView()
        {
            InitializeComponent();
        }
    }
}
