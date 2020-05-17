using System;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class ExportControl : UserControl
    {
        public ExportControl()
        {
            InitializeComponent();
        }

        public event EventHandler Cancel;
        public event EventHandler ExportDone;

        public void Initialise()
        {
            CustomSizesControl.ItemsSource = App.CurrentProject.CustomSizes;
            OptionsControl.ItemsSource = App.CurrentProject.StandardSizes;
            AppleGenerate.IsChecked = App.CurrentProject.IncludeCustomApple;
            AndroidGenerate.IsChecked = App.CurrentProject.IncludeCustomAndroid;
            WindowsGenerate.IsChecked = App.CurrentProject.IncludeCustomWindows;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        private async void ExportClick(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                CommitButtonText = "Select Output Folder"
            };
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder == null)
                return;
            ProgressRing.IsActive = true;
            WaitLayer.Visibility = Visibility.Visible;
            App.CurrentProject.IncludeCustomApple = AppleGenerate.IsChecked != null && AppleGenerate.IsChecked.Value;
            App.CurrentProject.IncludeCustomWindows = WindowsGenerate.IsChecked != null &&
                                                      WindowsGenerate.IsChecked.Value;
            App.CurrentProject.IncludeCustomAndroid = AndroidGenerate.IsChecked != null &&
                                                      AndroidGenerate.IsChecked.Value;
            App.CurrentProject.StandardSizes =
                App.CurrentProject.StandardSizes.Where(s => s.IsSelected).ToList();
            await IconExporter.Export(folder);
            await new MessageDialog("Export is complete").ShowAsync();
            await Launcher.LaunchFolderAsync(folder);
            ProgressRing.IsActive = false;
            WaitLayer.Visibility = Visibility.Collapsed;
            ExportDone?.Invoke(this, EventArgs.Empty);
        }

        private void AddCustomSize(object sender, RoutedEventArgs e)
        {
            App.CurrentProject.CustomSizes.Add(new CustomIconInfo {Size = 100d});
        }

        private void OnNumericBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Number1 && e.Key != VirtualKey.Number2 && e.Key != VirtualKey.Number3 &&
                e.Key != VirtualKey.Number4 && e.Key != VirtualKey.Number5 && e.Key != VirtualKey.Number6 &&
                e.Key != VirtualKey.Number7 && e.Key != VirtualKey.Number8 && e.Key != VirtualKey.Number9 &&
                e.Key != VirtualKey.Number0)
                e.Handled = true;
        }

        private void OnRemove(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var info = button?.Tag as CustomIconInfo;
            if (info == null)
                return;
            App.CurrentProject.CustomSizes.Remove(info);
        }

        private void OnSelectCheckChanged(object sender, RoutedEventArgs e)
        {
            if (App.CurrentProject == null || !SelectCheckbox.IsChecked.HasValue)
                return;
            var ch = SelectCheckbox.IsChecked.Value;
            foreach (var item in App.CurrentProject.StandardSizes) item.IsSelected = ch;
        }
    }
}