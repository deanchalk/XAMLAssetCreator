using System;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace XAMLAssetCreator.Converter
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return Colors.Transparent;
            var colorString = value.ToString();

            var property = typeof(Colors).GetRuntimeProperty(colorString);
            var color = property?.GetValue(null);
            if (color is Color) return color;

            try
            {
                return Color.FromArgb(
                    System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(1, 2), 16)),
                    System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(3, 2), 16)),
                    System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(5, 2), 16)),
                    System.Convert.ToByte(System.Convert.ToInt32(colorString.Substring(7, 2), 16)));
            }
            catch (Exception)
            {
                // Invalid value
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}