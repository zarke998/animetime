using AnimeTime.WPF.Assets;
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

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for ListViewExpandable.xaml
    /// </summary>
    public partial class ListViewExpandable : UserControl
    {

        #region Dependency Properties

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ListViewExpandable), new PropertyMetadata(false));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ListViewExpandable), new PropertyMetadata(String.Empty));



        #endregion

        private ListView _container;

        public ListViewExpandable()
        {
            InitializeComponent();
            Title = "Test";
            this.Loaded += ListViewExpandable_Loaded;
        }

        private void ListViewExpandable_Loaded(object sender, RoutedEventArgs e)
        {
            _container = this.Template.FindName("ItemsContainer", this) as ListView;
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
            _container.Items.Add(new { Title = "Test", Image = AssetsURIs.DefaultImage });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {

        }
    }
}
