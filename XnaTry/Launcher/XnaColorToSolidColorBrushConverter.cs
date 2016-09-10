using System;
using System.Windows.Data;
using System.Windows.Media;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Launcher
{
    [ValueConversion(typeof(XnaColor), typeof(SolidColorBrush))]
    public class XnaColorToSolidColorBrushConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var xnaColor = (XnaColor) value;
            return new SolidColorBrush(Color.FromArgb(xnaColor.A, xnaColor.R, xnaColor.G, xnaColor.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var color = ((SolidColorBrush) value).Color;
            return new XnaColor(color.R, color.G, color.B, color.A);
        }
    }
}