using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.AttachedProperties
{
    public static class UIElementAttachedProperties
    {


        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsFocused.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(UIElementAttachedProperties), new PropertyMetadata(false, OnIsFocusedChange));

        private static void OnIsFocusedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
