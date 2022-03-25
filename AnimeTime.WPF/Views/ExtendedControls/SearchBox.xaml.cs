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

namespace AnimeTime.WPF.Views.ExtendedControls
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchBox : TextBox
    {
        private TextBox _innerTb;

        public bool HasFocus => _innerTb?.IsFocused ?? false;
        
        public SearchBox()
        {
            InitializeComponent();
            this.GotFocus += Search_GotFocus;
            this.Loaded += SearchBox_Loaded;
        }

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            _innerTb = this.GetTemplateChild("tb") as TextBox;
            _innerTb.TextChanged += InnerTb_TextChanged;
        }

        private void InnerTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            this.Text = (e.OriginalSource as TextBox).Text;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            if (tb == this) e.Handled = true;
        }
    }
}
