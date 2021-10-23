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
    /// Interaction logic for NotificationButton.xaml
    /// </summary>
    public partial class NotificationButton : Button
    {
        #region Dependency Properties

        public int NumberOfNotifications
        {
            get { return (int)GetValue(NumberOfNotificationsProperty); }
            set { SetValue(NumberOfNotificationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberOfNotifications.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberOfNotificationsProperty =
            DependencyProperty.Register("NumberOfNotifications", typeof(int), typeof(NotificationButton), new PropertyMetadata(0));


        #endregion

        public NotificationButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void UserControl_Click(object sender, RoutedEventArgs e)
        {
            //e.Handled = e.OriginalSource is Button ? false : true;
            e.Handled = !(e.OriginalSource is Button) || e.OriginalSource == this;

            NumberOfNotifications = 2;
        }
    }
}
