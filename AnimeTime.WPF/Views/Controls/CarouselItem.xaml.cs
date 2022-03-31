﻿using System;
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
    /// Interaction logic for CarouselItem.xaml
    /// </summary>
    public partial class CarouselItem : UserControl
    {
        #region Depdendency Properties
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CarouselItem), new PropertyMetadata("Title"));
        #endregion

        public CarouselItem()
        {
            InitializeComponent();
            this.Loaded += CarouselItem_Loaded;
        }

        private void CarouselItem_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
