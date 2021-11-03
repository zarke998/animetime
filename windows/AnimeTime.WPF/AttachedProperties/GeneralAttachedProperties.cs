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
    }
}
