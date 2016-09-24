using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfServer.Windows.Converters
{
    [ValueConversion(typeof(float), typeof(string))]
    public class DecimalToIntegerPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0}" ,(int) ((float) value * 100f));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
