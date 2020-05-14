using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using XAMLAssetCreator.Converter;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class ColorPicker
    {
        public static string Transparent = "#00FFFFFF";
        private static readonly double PickerHeight = 150d;
        private static readonly double PickerWidth = 150d;
        private static readonly ColorConverter Converter = new ColorConverter();
        private int _alpha;
        private string _alphaEndColor;
        private string _alphaStartColor;
        private string _alphaString;
        private int _blue;
        private string _blueEndColor;
        private string _blueStartColor;
        private string _blueString;
        private string _color;
        private double _colorSpectrumPoint;
        private int _green;
        private string _greenEndColor;
        private string _greenStartColor;
        private string _greenString;
        private string _hueColor;
        private double _pickPointX;
        private double _pickPointY;
        private int _red;
        private string _redEndColor;
        private string _redStartColor;
        private string _redString;

        public ColorPicker()
        {
            Color = "#FFFF0000";
            PickPointX = 150d;
            PickPointY = 0d;
            ColorSpectrumPoint = 0d;
            UpdateColor(Colors.Red);
            UpdatePickPoint();
            InitializeComponent();
        }

        public string Color
        {
            get => _color;
            set
            {
                if (_color == value)
                    return;
                _color = value;
                OnPropertyChanged();
                UpdateColor((Color) Converter.Convert(value, typeof(Color), null, null));
                UpdatePickPoint();
            }
        }

        public string HueColor
        {
            get => _hueColor;
            set
            {
                _hueColor = value;
                OnPropertyChanged();
            }
        }

        public double PickPointX
        {
            get => _pickPointX;
            set
            {
                _pickPointX = value;
                OnPropertyChanged();
            }
        }

        public double PickPointY
        {
            get => _pickPointY;
            set
            {
                _pickPointY = value;
                OnPropertyChanged();
            }
        }

        public double ColorSpectrumPoint
        {
            get => _colorSpectrumPoint;
            set
            {
                _colorSpectrumPoint = value;
                OnPropertyChanged();
                var old = (Color) Converter.Convert(Color, typeof(Color), null, null);
                var hsv = ToHsv(old);
                hsv[0] = (float) (ColorSpectrumPoint * 360f / PickerHeight);
                var updated = FromHsv(hsv[0], hsv[1], hsv[2]);
                updated.A = old.A;
                UpdateColor(updated);
                var h = FromHsv(hsv[0], 1f, 1f);
                HueColor = $"#FF{h.R:X2}{h.G:X2}{h.B:X2}";
            }
        }

        public string RedString
        {
            get => _redString;
            set
            {
                _redString = value;
                OnPropertyChanged();
                if (int.TryParse(value, out var parsed)) Red = parsed;
            }
        }

        public string RedStartColor
        {
            get => _redStartColor;
            set
            {
                _redStartColor = value;
                OnPropertyChanged();
            }
        }

        public string RedEndColor
        {
            get => _redEndColor;
            set
            {
                _redEndColor = value;
                OnPropertyChanged();
            }
        }

        public string GreenString
        {
            get => _greenString;
            set
            {
                _greenString = value;
                OnPropertyChanged();
                if (int.TryParse(value, out var parsed)) Green = parsed;
            }
        }

        public string GreenStartColor
        {
            get => _greenStartColor;
            set
            {
                _greenStartColor = value;
                OnPropertyChanged();
            }
        }

        public string GreenEndColor
        {
            get => _greenEndColor;
            set
            {
                _greenEndColor = value;
                OnPropertyChanged();
            }
        }

        public string BlueString
        {
            get => _blueString;
            set
            {
                _blueString = value;
                OnPropertyChanged();
                if (int.TryParse(value, out var parsed)) Blue = parsed;
            }
        }

        public string BlueStartColor
        {
            get => _blueStartColor;
            set
            {
                _blueStartColor = value;
                OnPropertyChanged();
            }
        }

        public string BlueEndColor
        {
            get => _blueEndColor;
            set
            {
                _blueEndColor = value;
                OnPropertyChanged();
            }
        }

        public string AlphaString
        {
            get => _alphaString;
            set
            {
                _alphaString = value;
                OnPropertyChanged();
                if (int.TryParse(value, out var parsed)) Alpha = parsed;
            }
        }

        public string AlphaStartColor
        {
            get => _alphaStartColor;
            set
            {
                _alphaStartColor = value;
                OnPropertyChanged();
            }
        }

        public string AlphaEndColor
        {
            get => _alphaEndColor;
            set
            {
                _alphaEndColor = value;
                OnPropertyChanged();
            }
        }

        public int Red
        {
            get => _red;
            set
            {
                if (_red == value)
                    return;
                _red = value;
                OnPropertyChanged();
                var updated = (Color) Converter.Convert(Color, typeof(Color), null, null);
                updated.R = (byte) Math.Max(0, value);
                updated.R = Math.Min((byte) 0xff, updated.R);
                UpdateColor(updated);
                UpdatePickPoint();
            }
        }

        public int Green
        {
            get => _green;
            set
            {
                if (_green == value)
                    return;
                _green = value;
                OnPropertyChanged();
                var updated = (Color) Converter.Convert(Color, typeof(Color), null, null);
                updated.G = (byte) Math.Max(0, value);
                updated.G = Math.Min((byte) 0xff, updated.G);
                UpdateColor(updated);
                UpdatePickPoint();
            }
        }

        public int Blue
        {
            get => _blue;
            set
            {
                if (_blue == value)
                    return;
                _blue = value;
                OnPropertyChanged();
                var updated = (Color) Converter.Convert(Color, typeof(Color), null, null);
                updated.B = (byte) Math.Max(0, value);
                updated.B = Math.Min((byte) 0xff, updated.B);
                UpdateColor(updated);
                UpdatePickPoint();
            }
        }

        public int Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha == value)
                    return;
                _alpha = value;
                OnPropertyChanged();
                var updated = (Color) Converter.Convert(Color, typeof(Color), null, null);
                updated.A = (byte) Math.Max(0, value);
                updated.A = Math.Min((byte) 0xff, updated.A);
                UpdateColor(updated);
                UpdatePickPoint();
            }
        }

        public void UpdatePickPoint()
        {
            var hsv = ToHsv((Color) Converter.Convert(Color, typeof(Color), null, null));
            PickPointX = PickerWidth * hsv[1];
            PickPointY = PickerHeight * (1 - hsv[2]);
            ColorSpectrumPoint = PickerHeight * hsv[0] / 360f;
        }

        private void OnHuePressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeHue(e.GetCurrentPoint(colorSpectrum).Position.Y);
            colorSpectrum.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                ChangeHue(args.GetCurrentPoint(colorSpectrum).Position.Y);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                colorSpectrum.ReleasePointerCapture(args.Pointer);
                ChangeHue(args.GetCurrentPoint(colorSpectrum).Position.Y);
                colorSpectrum.PointerMoved -= Moved;
                colorSpectrum.PointerReleased -= Released;
            }

            colorSpectrum.PointerMoved += Moved;
            colorSpectrum.PointerReleased += Released;
        }

        private void ChangeHue(double y)
        {
            var py = Math.Max(0d, y);
            py = Math.Min(colorSpectrum.ActualHeight, py);
            ColorSpectrumPoint = Math.Round(py, MidpointRounding.AwayFromZero);
        }

        private void PickColor(Point point)
        {
            var px = Math.Max(0d, point.X);
            px = Math.Min(PickerCanvas.ActualWidth, px);
            var py = Math.Max(0d, point.Y);
            py = Math.Min(PickerCanvas.ActualHeight, py);
            PickPointX = Math.Round(px, MidpointRounding.AwayFromZero);
            PickPointY = Math.Round(py, MidpointRounding.AwayFromZero);
            OnPickPointChanged();
        }

        public void OnPickPointChanged()
        {
            var old = (Color) Converter.Convert(HueColor, typeof(Color), null, null);
            var hsv = ToHsv(old);
            var updated = FromHsv(hsv[0], (float) (PickPointX / PickerWidth), 1f - (float) (PickPointY / PickerHeight));
            updated.A = old.A;
            UpdateColor(updated);
        }

        public void UpdateColor(Color color)
        {
            Color = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            Alpha = color.A;
            AlphaString = Alpha.ToString();
            Red = color.R;
            RedString = Red.ToString();
            Green = color.G;
            GreenString = Green.ToString();
            Blue = color.B;
            BlueString = Blue.ToString();
            RedStartColor = $"#{0xff:X2}{0:X2}{color.G:X2}{color.B:X2}";
            RedEndColor = $"#{0xff:X2}{0xff:X2}{color.G:X2}{color.B:X2}";
            GreenStartColor = $"#{0xff:X2}{color.R:X2}{0:X2}{color.B:X2}";
            GreenEndColor = $"#{0xff:X2}{color.R:X2}{0xff:X2}{color.B:X2}";
            BlueStartColor = $"#{0xff:X2}{color.R:X2}{color.G:X2}{0:X2}";
            BlueEndColor = $"#{0xff:X2}{color.R:X2}{color.G:X2}{0xff:X2}";
            AlphaStartColor = $"#{0:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            AlphaEndColor = $"#{0xff:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            var hsv = ToHsv(color);
            var h = FromHsv(hsv[0], 1f, 1f);
            HueColor = $"#FF{h.R:X2}{h.G:X2}{h.B:X2}";
        }

        private static Color FromHsv(float hue, float saturation, float brightness)
        {
            if (Math.Abs(saturation) < double.Epsilon)
            {
                var c = (byte) Math.Round(brightness * 255f, MidpointRounding.AwayFromZero);
                return ColorHelper.FromArgb(0xff, c, c, c);
            }

            var hi = (int) (hue / 60f) % 6;
            var f = hue / 60f - (int) (hue / 60d);
            var p = brightness * (1 - saturation);
            var q = brightness * (1 - f * saturation);
            var t = brightness * (1 - (1 - f) * saturation);

            float r, g, b;
            switch (hi)
            {
                case 0:
                    r = brightness;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = brightness;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = brightness;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = brightness;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = brightness;
                    break;

                case 5:
                    r = brightness;
                    g = p;
                    b = q;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return ColorHelper.FromArgb(
                0xff,
                (byte) Math.Round(r * 255d),
                (byte) Math.Round(g * 255d),
                (byte) Math.Round(b * 255d));
        }

        private static float[] ToHsv(Color color)
        {
            var rgb = new[]
            {
                color.R / 255f, color.G / 255f, color.B / 255f
            };

            // RGB to HSV
            var max = rgb.Max();
            var min = rgb.Min();

            float h, s, v;
            if (Math.Abs(max - min) < float.Epsilon)
                h = 0f;
            else if (Math.Abs(max - rgb[0]) < float.Epsilon)
                h = (60f * (rgb[1] - rgb[2]) / (max - min) + 360f) % 360f;
            else if (Math.Abs(max - rgb[1]) < float.Epsilon)
                h = 60f * (rgb[2] - rgb[0]) / (max - min) + 120f;
            else
                h = 60f * (rgb[0] - rgb[1]) / (max - min) + 240f;

            if (Math.Abs(max) < float.Epsilon)
                s = 0f;
            else
                s = (max - min) / max;
            v = max;

            return new[] {h, s, v};
        }

        private void OnPickerPressed(object sender, PointerRoutedEventArgs e)
        {
            PickColor(e.GetCurrentPoint(PickerCanvas).Position);
            PickerCanvas.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                PickColor(args.GetCurrentPoint(PickerCanvas).Position);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                PickerCanvas.ReleasePointerCapture(args.Pointer);
                PickColor(args.GetCurrentPoint(PickerCanvas).Position);
                PickerCanvas.PointerMoved -= Moved;
                PickerCanvas.PointerReleased -= Released;
            }

            PickerCanvas.PointerMoved += Moved;
            PickerCanvas.PointerReleased += Released;
        }

        private void OnRedPressed(object sender, PointerRoutedEventArgs e)
        {
            Red = ArrangeArgb(e.GetCurrentPoint(red).Position.X, red.ActualWidth);
            red.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                Red = ArrangeArgb(e.GetCurrentPoint(red).Position.X, red.ActualWidth);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                red.ReleasePointerCapture(args.Pointer);
                Red = ArrangeArgb(e.GetCurrentPoint(red).Position.X, red.ActualWidth);
                red.PointerMoved -= Moved;
                red.PointerReleased -= Released;
            }

            red.PointerMoved += Moved;
            red.PointerReleased += Released;
        }

        private int ArrangeArgb(double x, double max)
        {
            var px = x * 255d / max;
            px = Math.Max(0d, px);
            px = Math.Min(255d, px);

            return (int) Math.Round(px, MidpointRounding.AwayFromZero);
        }

        private void OnGreenPressed(object sender, PointerRoutedEventArgs e)
        {
            Green = ArrangeArgb(e.GetCurrentPoint(green).Position.X, green.ActualWidth);
            green.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                Green = ArrangeArgb(e.GetCurrentPoint(green).Position.X, green.ActualWidth);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                green.ReleasePointerCapture(args.Pointer);
                Green = ArrangeArgb(e.GetCurrentPoint(green).Position.X, green.ActualWidth);
                green.PointerMoved -= Moved;
                green.PointerReleased -= Released;
            }

            green.PointerMoved += Moved;
            green.PointerReleased += Released;
        }

        private void OnBluePressed(object sender, PointerRoutedEventArgs e)
        {
            Blue = ArrangeArgb(e.GetCurrentPoint(blue).Position.X, blue.ActualWidth);
            blue.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                Blue = ArrangeArgb(e.GetCurrentPoint(blue).Position.X, blue.ActualWidth);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                blue.ReleasePointerCapture(args.Pointer);
                Blue = ArrangeArgb(e.GetCurrentPoint(blue).Position.X, blue.ActualWidth);
                blue.PointerMoved -= Moved;
                blue.PointerReleased -= Released;
            }

            blue.PointerMoved += Moved;
            blue.PointerReleased += Released;
        }

        private void OnAlphaPressed(object sender, PointerRoutedEventArgs e)
        {
            Alpha = ArrangeArgb(e.GetCurrentPoint(alpha).Position.X, alpha.ActualWidth);
            alpha.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                Alpha = ArrangeArgb(e.GetCurrentPoint(alpha).Position.X, alpha.ActualWidth);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                alpha.ReleasePointerCapture(args.Pointer);
                Alpha = ArrangeArgb(e.GetCurrentPoint(alpha).Position.X, alpha.ActualWidth);
                alpha.PointerMoved -= Moved;
                alpha.PointerReleased -= Released;
            }

            alpha.PointerMoved += Moved;
            alpha.PointerReleased += Released;
        }

        private void OnColorStringKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Accept)
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                e.Handled = true;
            }
        }
    }
}