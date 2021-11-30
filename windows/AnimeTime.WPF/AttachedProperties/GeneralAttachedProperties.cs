using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.AttachedProperties
{
    public static class GeneralAttachedProperties
    {

        #region RotationAngle
        public static int GetRotationAngle(DependencyObject obj)
        {
            return (int)obj.GetValue(RotationAngleProperty);
        }

        public static void SetRotationAngle(DependencyObject obj, int value)
        {
            obj.SetValue(RotationAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for RotationAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.RegisterAttached("RotationAngle", typeof(int), typeof(GeneralAttachedProperties), new PropertyMetadata(0));
        #endregion


        #region Margins
        public static double GetMarginTop(DependencyObject obj)
        {
            return (double)obj.GetValue(MarginTopProperty);
        }

        public static void SetMarginTop(DependencyObject obj, double value)
        {
            obj.SetValue(MarginTopProperty, value);
        }

        // Using a DependencyProperty as the backing store for MarginTop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginTopProperty =
            DependencyProperty.RegisterAttached("MarginTop", typeof(double), typeof(GeneralAttachedProperties), new PropertyMetadata(0.0, TopChangedCallback));

        private static void TopChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as FrameworkElement;
            el.Margin = new Thickness(el.Margin.Left,
                                      (double)e.NewValue,
                                      el.Margin.Right,
                                      el.Margin.Bottom);
        }
        #endregion
    }
}
