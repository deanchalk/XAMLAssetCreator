using System;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class ForegroundColorWidget
    {
        private Brush _color;
        private string _colorString = "#B10821";

        public ForegroundColorWidget()
        {
            _color = GetColorFromString("#B10821");
            InitializeComponent();
        }

        public Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public string ColorString
        {
            get => _colorString;
            set
            {
                _colorString = value;
                OnPropertyChanged();
            }
        }

        private Brush GetColorFromString(string color)
        {
            if (color.StartsWith("#"))
            {
                if (color.Length == 7) color = color.Replace("#", "#FF");
                var argColor = Windows.UI.Color.FromArgb(
                    Convert.ToByte(Convert.ToInt32(color.Substring(1, 2), 16)),
                    Convert.ToByte(Convert.ToInt32(color.Substring(3, 2), 16)),
                    Convert.ToByte(Convert.ToInt32(color.Substring(5, 2), 16)),
                    Convert.ToByte(Convert.ToInt32(color.Substring(7, 2), 16)));
                return new SolidColorBrush(argColor);
            }

            var prop = typeof(Colors).GetRuntimeProperty(color);
            var c = prop?.GetValue(null);
            if (c is Color ccolor) return new SolidColorBrush(ccolor);
            return new SolidColorBrush(Colors.Black);
        }

        public event EventHandler<string> ForegroundChanged;

        private async void OnPickColorClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog(ColorString);
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Secondary)
                return;
            var prop = typeof(Colors).GetRuntimeProperty(dialog.Color);
            var val = prop?.GetValue(null);
            if (val is Color cVal)
            {
                Color = new SolidColorBrush(cVal);
                return;
            }

            var color = Windows.UI.Color.FromArgb(
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(1, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(3, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(5, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(7, 2), 16)));
            Color = new SolidColorBrush(color);
            ColorString = dialog.Color;
            ForegroundChanged?.Invoke(this, ColorString);
        }
    }
}