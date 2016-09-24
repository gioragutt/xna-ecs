using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfServer.Windows.Converters
{
    [ValueConversion(typeof(Guid), typeof(string))]
    public class PlayerGuidToDisplayerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().Substring(0, 6) ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
