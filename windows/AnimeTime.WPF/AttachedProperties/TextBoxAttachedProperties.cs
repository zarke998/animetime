using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AnimeTime.WPF.AttachedProperties
{
    public static class TextBoxAttachedProperties
    {
        #region Placeholder
        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }
        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBoxAttachedProperties), new PropertyMetadata(String.Empty, OnPlaceholder_Change));

        private static void OnPlaceholder_Change(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;

            if (textBox == null)
                return;

            textBox.Loaded -= TextBox_Loaded;
            textBox.Loaded += TextBox_Loaded;

            textBox.GotFocus -= TextBox_GotFocus;
            textBox.GotFocus += TextBox_GotFocus;

            textBox.LostFocus -= TextBox_LostFocus;
            textBox.LostFocus += TextBox_LostFocus;

            textBox.TextChanged -= TextBox_TextChanged;
            textBox.TextChanged += TextBox_TextChanged;
        }
        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if(!textBox.IsFocused && textBox.Text == String.Empty)
                textBox.Text = GetPlaceholder(textBox);
        }
        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            if(textBox.Text == String.Empty)
                textBox.Text = GetPlaceholder(textBox);
        }
        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            if(textBox.Text == GetPlaceholder(textBox))
                textBox.Text = String.Empty;
        }
        private static void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text = GetPlaceholder(textBox);
        }
        #endregion
    }
}
