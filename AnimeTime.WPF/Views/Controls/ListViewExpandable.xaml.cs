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
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ListViewExpandable), new PropertyMetadata(false));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ListViewExpandable), new PropertyMetadata("Section"));
        #endregion

        #region Commands

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register("ItemSelectedCommand", typeof(ICommand), typeof(ListViewExpandable), new PropertyMetadata(null));

        #endregion

        #region Templated Elements
        private ListView _container;
        #endregion


        public ListViewExpandable()
        {
            InitializeComponent();
            this.Loaded += ListViewExpandable_Loaded;
        }

        private void ListViewExpandable_Loaded(object sender, RoutedEventArgs e)
        {
            _container = this.Template.FindName("ItemsContainer", this) as ListView;
            _container.SelectionChanged += _container_SelectionChanged;

            if (_container != null)
            {
                LoadTestData();
            }
        }

        private void _container_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_container.SelectedItem == null) return;

            RaiseItemSelectedCommand(e.AddedItems[0]);

            _container.SelectedItem = null;
        }

        private void RaiseItemSelectedCommand(object data)
        {
            if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(null))
                ItemSelectedCommand.Execute(data);
        }

        private void LoadTestData()
        {
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

        #region Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void ListViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(null))
                ItemSelectedCommand.Execute(null);
        }

        #endregion

        private void ItemsContainer_Selected(object sender, RoutedEventArgs e)
        {
            if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(null))
                ItemSelectedCommand.Execute(null);
        }
    }
}
