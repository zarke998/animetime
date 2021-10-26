using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : UserControl
    {
        #region Dependency Properties


        public ObservableCollection<Notification> NotificationList
        {
            get { return (ObservableCollection<Notification>)GetValue(NotificationListProperty); }
            set { SetValue(NotificationListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationListProperty =
            DependencyProperty.Register("NotificationList", typeof(ObservableCollection<Notification>), typeof(Notifications), new PropertyMetadata(new ObservableCollection<Notification>(), OnNotificationListChanged));

        private static void OnNotificationListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldVal = e.OldValue as ObservableCollection<Notification>;
            var newVal = e.NewValue as ObservableCollection<Notification>;
            var self = d as Notifications;

            if(oldVal == null)
            {
                newVal.CollectionChanged += self.NotificationList_SizeChanged;
            }
            if(newVal == null)
            {
                oldVal.CollectionChanged -= self.NotificationList_SizeChanged;
            }
            if(oldVal != null && newVal != null)
            {
                oldVal.CollectionChanged -= self.NotificationList_SizeChanged;
                newVal.CollectionChanged += self.NotificationList_SizeChanged;
            }

            self.UpdateNoNotificationVisibility();
            self.UpdateNotificationLvwVisibility();
        }

        public bool PanelVisible
        {
            get { return (bool)GetValue(PanelVisibleProperty); }
            set { SetValue(PanelVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelVisibleProperty =
            DependencyProperty.Register("PanelVisible", typeof(bool), typeof(Notifications), new PropertyMetadata(false));

        #endregion
        public Notifications()
        {
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PanelVisible = !PanelVisible;
        }

        private void NotificationList_SizeChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNoNotificationVisibility();
            UpdateNotificationLvwVisibility();
        }

        private void UpdateNotificationLvwVisibility()
        {
            var child = this.GetTemplateChild("NoNotificationsInfo") as StackPanel;
            child.Visibility = NotificationList.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateNoNotificationVisibility()
        {
            var child = this.GetTemplateChild("NotificationsLvw") as StackPanel;
            child.Visibility = NotificationList.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Episode { get; set; }
    }
}
