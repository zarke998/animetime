using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.WPF.Views
{
    public static class ExtendedProperties
    {

        #region CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ExtendedProperties));
        public static string GetCornerRadius(UIElement element)
        {
            return (string)element.GetValue(CornerRadiusProperty);
        }
        public static void SetCornerRadius(UIElement element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusTopRightProperty = DependencyProperty.RegisterAttached("CornerRadiusTopRight", typeof(string), typeof(ExtendedProperties), new FrameworkPropertyMetadata("0", FrameworkPropertyMetadataOptions.AffectsRender));
        public static string GetCornerRadiusTopRight(UIElement element)
        {
            return (string)element.GetValue(CornerRadiusTopRightProperty);
        }
        public static void SetCornerRadiusTopRight(UIElement element, string value)
        {
            element.SetValue(CornerRadiusTopRightProperty, value);
        }
        #endregion
    }
}
