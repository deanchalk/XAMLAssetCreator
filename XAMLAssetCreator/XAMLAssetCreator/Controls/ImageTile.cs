using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace XAMLAssetCreator.Controls
{
    public sealed class ImageTile : Panel
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(ImageSource),
            typeof(ImageTile),
            new PropertyMetadata(null, OnSourceChanged));

        private Image _sourceImage;

        public ImageTile()
        {
            Unloaded += OnUnloaded;
        }


        public ImageSource Source
        {
            get => (ImageSource) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as ImageTile;
            if (panel == null) return;
            panel._sourceImage = new Image
            {
                Source = panel.Source,
                UseLayoutRounding = false,
                Stretch = Stretch.None
            };
            panel._sourceImage.ImageOpened += panel.OnSourceImageOpened;
            panel._sourceImage.ImageFailed += panel.OnSourceImageFailed;
            panel.Children.Add(panel._sourceImage);
        }

        private void OnSourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            _sourceImage.ImageOpened -= OnSourceImageOpened;
            _sourceImage.ImageFailed -= OnSourceImageFailed;
            _sourceImage = null;
            Children.Clear();
        }

        private void OnSourceImageOpened(object sender, RoutedEventArgs e)
        {
            _sourceImage.ImageOpened -= OnSourceImageOpened;
            _sourceImage.ImageFailed -= OnSourceImageFailed;
            _sourceImage = null;
            InvalidateArrange();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= OnUnloaded;
            if (_sourceImage == null) return;
            _sourceImage.ImageFailed -= OnSourceImageFailed;
            _sourceImage.ImageOpened -= OnSourceImageOpened;
            _sourceImage = null;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (!(Source is BitmapSource bmp)) return base.ArrangeOverride(finalSize);

            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;
            if (width == 0 || height == 0) return base.ArrangeOverride(finalSize);

            var index = 0;
            for (double x = 0; x < finalSize.Width; x += width)
            for (double y = 0; y < finalSize.Height; y += height)
            {
                Image image;
                if (Children.Count > index)
                {
                    image = (Image) Children[index];
                    image.Source = bmp;
                }
                else
                {
                    image = new Image
                    {
                        Source = bmp,
                        UseLayoutRounding = false,
                        Stretch = Stretch.None
                    };
                    Children.Add(image);
                }

                image.Measure(new Size(width, height));
                image.Arrange(new Rect(x, y, width, height));
                index++;
            }

            var count = Children.Count;
            for (var i = index; i < count; i++) Children.RemoveAt(index);

            Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, finalSize.Width, finalSize.Height)
            };
            return base.ArrangeOverride(finalSize);
        }
    }
}