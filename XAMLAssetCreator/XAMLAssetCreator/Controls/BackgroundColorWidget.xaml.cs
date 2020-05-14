﻿using System;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class BackgroundColorWidget
    {
        private Brush _color;
        private string _colorString;

        public BackgroundColorWidget()
        {
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

        public event EventHandler<string> BackgroundChanged;

        private void BackgroundTransparentChanged(object sender, RoutedEventArgs e)
        {
            if (BackgroundTransparent.IsChecked == null)
                return;
            var trans = BackgroundTransparent.IsChecked.Value;
            if (trans && ColorRect != null && ColorTextBox != null && PickButton != null)
            {
                ColorRect.Visibility = Visibility.Collapsed;
                ColorTextBox.IsEnabled = false;
                PickButton.IsEnabled = false;
                ColorString = ColorPicker.Transparent;
            }
            else if (ColorRect != null && ColorTextBox != null && PickButton != null)
            {
                ColorRect.Visibility = Visibility.Visible;
                ColorTextBox.IsEnabled = true;
                PickButton.IsEnabled = true;
            }
        }

        private async void OnPickColorClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog(ColorString);
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Secondary)
                return;
            var prop = typeof(Colors).GetRuntimeProperty(dialog.Color);
            var val = prop?.GetValue(null);
            if (val is Color cval)
            {
                Color = new SolidColorBrush(cval);
                return;
            }

            var color = Windows.UI.Color.FromArgb(
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(1, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(3, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(5, 2), 16)),
                Convert.ToByte(Convert.ToInt32(dialog.Color.Substring(7, 2), 16)));
            Color = new SolidColorBrush(color);
            ColorString = dialog.Color;
            BackgroundChanged?.Invoke(this, ColorString);
        }
    }
}