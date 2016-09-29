using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfServer.Windows.Converters
{
    [ValueConversion(typeof(string), typeof(IList<string>))]
    public class StringToStringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value.ToString()) ? null : value.ToString().Split(' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
