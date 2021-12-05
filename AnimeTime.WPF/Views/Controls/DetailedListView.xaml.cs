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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for DetailedListView.xaml
    /// </summary>
    public partial class DetailedListView : UserControl
    {
        #region Dependency Properties


        public ObservableCollection<DetailedItem> Items
        {
            get { return (ObservableCollection<DetailedItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<DetailedItem>), typeof(DetailedListView), new PropertyMetadata(new ObservableCollection<DetailedItem>()));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DetailedListView), new PropertyMetadata("Title"));


        #endregion

        public DetailedListView()
        {
            InitializeComponent();
            Items.Add(new DetailedItem()
            {
                Title = "Test",
                Rating = 9.1f,
                ReleaseYear = 1999,
                ContentType = "TV",
                Description = "This is a description..."
            });
            Items.Add(new DetailedItem()
            {
                Title = "Test",
                Rating = 9.1f,
                ReleaseYear = 1999,
                ContentType = "TV",
                Description = "This is a description...This is a description...This is a description...This is a description...This is a description...This is a description...This is a description...This is a description...This is a description..."
            });
        }
    }

    public class DetailedItem
    {
        public string Title { get; set; }
        public float Rating { get; set; }
        public int ReleaseYear { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
    }
}
