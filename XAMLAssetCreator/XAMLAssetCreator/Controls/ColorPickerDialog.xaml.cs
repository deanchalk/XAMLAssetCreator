using Windows.UI.Xaml.Controls;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class ColorPickerDialog : ContentDialog
    {
        public ColorPickerDialog(string startColor)
        {
            InitializeComponent();
            ColorPicker.Color = startColor;
        }

        public string Color { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Color = ColorPicker.Color;
        }
    }
}