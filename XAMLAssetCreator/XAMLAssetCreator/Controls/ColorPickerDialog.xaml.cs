using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class ColorPickerDialog : ContentDialog
    {
        public ColorPickerDialog(string startColor)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(startColor))
            {
                if (startColor == Constants.TransparentColor) startColor = Constants.PickerDefaultColor;
                ColorPicker.Color = ColorHelper.ToColor(startColor);
            }
        }

        public string Color { get; set; }

        private void SetColorClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Color = ColorHelper.ToHex(ColorPicker.Color);
        }
    }
}