using AnimeTime.Utilities.Colors;
using AnimeTime.Utilities.MathHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AnimeTime.WPF.Converters
{
    public class ColorLightnessConverter : IValueConverter
    {
        /// <summary>
        /// Adjust brightness for solid brush.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Lightness factor. Values > 0 for brightening, < 0 for darkening the color.</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float lightness = float.Parse(parameter.ToString());

            var inputBrush = value as SolidColorBrush;

            if (inputBrush == null) 
                return null;
                //throw new ArgumentException("Target not a solid color brush.");

            var hsla = inputBrush.Color.ToHslaColor();

            return new SolidColorBrush(hsla.SetLightness(MathUtil.Clamp(hsla.L + lightness, 0, HslaColor.LIGHTNESS_MAX)).ToMediaColor());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
