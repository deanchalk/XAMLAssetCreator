using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator.Controls
{
    public sealed class SkiaView : Canvas
    {
        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Color), typeof(SkiaView),
                new PropertyMetadata(Colors.Black, OnColourPropertyValueChanged));

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register("IconBackground", typeof(Color), typeof(SkiaView),
                new PropertyMetadata(Colors.Transparent, OnColourPropertyValueChanged));

        public static readonly DependencyProperty BackgroundTypeProperty =
            DependencyProperty.Register("BackgroundType", typeof(BackgroundType), typeof(SkiaView),
                new PropertyMetadata(BackgroundType.None, OnPropertyValueChanged));


        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(int), typeof(SkiaView),
                new PropertyMetadata(0, OnPropertyValueChanged));


        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(SkiaView),
                new PropertyMetadata(100, OnPropertyValueChanged));


        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register("PathData", typeof(string), typeof(SkiaView),
                new PropertyMetadata(string.Empty, OnPropertyValueChanged));


        public static readonly DependencyProperty LeftRightOffsetProperty =
            DependencyProperty.Register("LeftRightOffset", typeof(double), typeof(SkiaView),
                new PropertyMetadata(0d, OnPropertyValueChanged));

        public static readonly DependencyProperty UpDownOffsetProperty =
            DependencyProperty.Register("UpDownOffset", typeof(double), typeof(SkiaView),
                new PropertyMetadata(0d, OnPropertyValueChanged));

        public SkiaView()
        {

        }

        public double LeftRightOffset
        {
            get => (double)GetValue(LeftRightOffsetProperty);
            set => SetValue(LeftRightOffsetProperty, value);
        }

        public double UpDownOffset
        {
            get => (double)GetValue(UpDownOffsetProperty);
            set => SetValue(UpDownOffsetProperty, value);
        }


        public Color IconForeground
        {
            get => (Color)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        public Color IconBackground
        {
            get => (Color)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        public BackgroundType BackgroundType
        {
            get => (BackgroundType)GetValue(BackgroundTypeProperty);
            set => SetValue(BackgroundTypeProperty, value);
        }

        public int Padding
        {
            get => (int)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public string PathData
        {
            get => (string)GetValue(PathDataProperty);
            set => SetValue(PathDataProperty, value);
        }

        private static void OnColourPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var skiaView = d as SkiaView;
            if (skiaView == null)
            {
                return;
            }
            if (!(e.OldValue is Color) || !(e.NewValue is Color))
            {
                return;
            }

            var c1 = (Color)e.OldValue;
            var c2 = (Color)e.NewValue;
            if (c1.Equals(c2))
            {
                return;
            }
            skiaView.RepaintBackground();
        }

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var skiaView = d as SkiaView;
            if (skiaView == null)
            {
                return;
            }
            if (e.OldValue.Equals(e.NewValue))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(skiaView.PathData))
            {
                return;
            }
            skiaView.RepaintBackground();
        }

        private void RepaintBackground()
        {
            Width = Size;
            Height = Size;
            if (string.IsNullOrEmpty(PathData))
                return;
            var pixels = SkiaIconFactory.CreateIconBytes(PathData, Size, Padding * (Size / 100),
                Utility.HexConverter(IconBackground),
                Utility.HexConverter(IconForeground), BackgroundType, LeftRightOffset * (Size / 100),
                UpDownOffset * (Size / 100d));
            var bitmap = new WriteableBitmap(Size, Size);

            var stream = bitmap.PixelBuffer.AsStream();
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(pixels, 0, pixels.Length);

            bitmap.Invalidate();

            var b = bitmap;
            Background = new ImageBrush
            {
                ImageSource = b,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center,
                Stretch = Stretch.None
            };
        }

        public static async Task CreateImageInstance(string pathData, double size, double pad, string background,
            string foreground, BackgroundType bacgroundType, double leftRightOffset, double upDownOffset,
            StorageFolder folder, string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var data = SkiaIconFactory.CreateData(pathData, size, pad * (size / 100), background, foreground,
                bacgroundType, leftRightOffset * (size / 100), upDownOffset * (size / 100));
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                data.SaveTo(stream);
            }
        }
    }
}