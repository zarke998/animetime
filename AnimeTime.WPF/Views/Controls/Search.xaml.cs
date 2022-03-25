using AnimeTime.Services.DTO;
using AnimeTime.WPF.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {

        #region Dependency Properties
        public ICommand KeywordChangedCommand
        {
            get { return (ICommand)GetValue(KeywordChangedCommandProperty); }
            set { SetValue(KeywordChangedCommandProperty, value); }
        }
        public static readonly DependencyProperty KeywordChangedCommandProperty =
            DependencyProperty.Register(nameof(KeywordChangedCommand), typeof(ICommand), typeof(Search), new PropertyMetadata(null));

        public IEnumerable SearchResults
        {
            get { return (IEnumerable)GetValue(SearchResultsProperty); }
            set { SetValue(SearchResultsProperty, value); }
        }
        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register(nameof(SearchResults), typeof(IEnumerable), typeof(Search), new PropertyMetadata(new PropertyChangedCallback(SearchResultsChanged)));

        private static void SearchResultsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as Search;
            var results = (e.NewValue as IEnumerable<object>);
            if (self.SearchBox.HasFocus)
            {
                self.IsSearchResultsShowed = results.Count() > 0;
            }

            if(e.NewValue is INotifyCollectionChanged observable)
            {
                observable.CollectionChanged += self.SearchResults_CollectionChanged;
            }
        }
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register(nameof(ItemSelectedCommand), typeof(ICommand), typeof(Search), new PropertyMetadata(null));

        #endregion

        #region Properties
        public bool IsSearchResultsShowed
        {
            get { return (bool)GetValue(IsSearchResultsShowedProperty); }
            set { SetValue(IsSearchResultsShowedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSearchResultsShowed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSearchResultsShowedProperty =
            DependencyProperty.Register("IsSearchResultsShowed", typeof(bool), typeof(Search), new PropertyMetadata(false));


        #endregion
        public Search()
        {
            InitializeComponent();
        }

        private void SearchResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SearchBox.HasFocus)
                IsSearchResultsShowed = e.NewItems?.Count > 0;
        }

        #region Events
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (KeywordChangedCommand == null || !KeywordChangedCommand.CanExecute(null)) return;

            KeywordChangedCommand.Execute(SearchBox.Text);
        }

        private void SearchResultsLvw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SearchResultsLvw.SelectedItem as AnimeSearchDTO;

            if (selectedItem == null || ItemSelectedCommand == null || !ItemSelectedCommand.CanExecute(null)) return;

            ItemSelectedCommand.Execute(selectedItem.Id);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchResults.Cast<object>().Count() > 0)
                IsSearchResultsShowed = true;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsSearchResultsShowed = false;
        }

        #endregion
    }
}
