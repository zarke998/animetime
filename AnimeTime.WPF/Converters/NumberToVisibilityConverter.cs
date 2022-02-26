using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AnimeTime.WPF.Converters
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return null;
            int val = (int)value;

            return val == 0 ? GetDefaultVisibility(parameter) : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Visibility GetDefaultVisibility(object parameter)
        {
            var defaultVisibility = Visibility.Hidden;

            if (parameter == null)
                return defaultVisibility;

            if (parameter.ToString() == "Collapsed")
                defaultVisibility = Visibility.Collapsed;

            return defaultVisibility;
        }
    }
}
