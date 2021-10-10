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

        // Using a DependencyProperty as the backing store for KeywordChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeywordChangedCommandProperty =
            DependencyProperty.Register(nameof(KeywordChangedCommand), typeof(ICommand), typeof(Search), new PropertyMetadata(null));
        #endregion

        public Search()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (KeywordChangedCommand == null || !KeywordChangedCommand.CanExecute(null)) return;

            KeywordChangedCommand.Execute(SearchBox.Text);
        }
    }
}
