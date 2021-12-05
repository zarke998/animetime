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
    public class MarginToSingleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Thickness margin)
            {
                switch(parameter.ToString())
                {
                    case "Left": return margin.Left;
                    case "Top": return margin.Top;
                    case "Right": return margin.Right;
                    case "Bottom": return margin.Bottom;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
