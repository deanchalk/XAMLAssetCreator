using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using XAMLAssetCreator.Controls;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ExportControl.Cancel += (o, e) =>
            {
                Canvas.SetZIndex(ExportIconsLayer, -1);
                EnableBarButtons();
            };
            ForegroundColorWidget.ForegroundChanged += OnForegroundColorChanged;
            BackgroundColorWidget.BackgroundChanged += OnBackgroundColorChanged;
        }

        private void OnBackgroundColorChanged(object sender, string e)
        {
            if (App.CurrentProject == null)
                return;
            var selected = App.CurrentProject.Icons.Where(i => i.AppSelected).ToList();
            foreach (var icon in selected) icon.BackgroundColor = e;
        }

        private void OnForegroundColorChanged(object sender, string e)
        {
            if (App.CurrentProject == null)
                return;
            var selected = App.CurrentProject.Icons.Where(i => i.AppSelected).ToList();
            foreach (var icon in selected) icon.ForegroundColor = e;
        }

        private async void ExportProject_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await CheckIconNames()) return;
            Canvas.SetZIndex(ExportIconsLayer, 1);
            ExportControl.Initialise();
            DisableBarButtons();
        }

        private async Task<bool> CheckIconNames()
        {
            if (!App.CurrentProject.Icons.GroupBy(i => i.Name).Any(g => g.Count() > 1))
                return true;
            var dialog = new MessageDialog("All icons must have a unique name");
            await dialog.ShowAsync();
            return false;
        }

        private async void NewProjectClick(object sender, RoutedEventArgs e)
        {
            var newProjectDialog = new NewProjectDialog();
            await newProjectDialog.ShowAsync();
            if (string.IsNullOrWhiteSpace(App.CurrentProject?.Name)) return;
            var addIconsDialog = new AddIconsDialog();
            await addIconsDialog.ShowAsync();
            IntroGrid.Visibility = Visibility.Collapsed;
            IconsControl.ItemsSource = App.CurrentProject.Icons;
            EnableBarButtons();
            DeselectAll.IsEnabled = true;
            SelectAll.IsEnabled = true;
            AddIcons.IsEnabled = true;
            RemoveSelected.IsEnabled = true;
            ExportProject.IsEnabled = true;
            SaveProject.IsEnabled = true;
            SaveProjectAs.IsEnabled = true;
        }

        private void OnBackgroundTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.CurrentProject == null)
                return;
            var selected = BackgroundTypeSelector.SelectedItem as ComboBoxItem;
            if (selected == null)
                return;
            var iconBackType = (BackgroundType) Enum.Parse(typeof(BackgroundType), selected.Content as string);
            var selectedIcons = App.CurrentProject.Icons.Where(i => i.AppSelected).ToList();
            foreach (var icon in selectedIcons)
            {
                icon.BackType = iconBackType;
                if (icon.BackType == BackgroundType.Circle && icon.Padding == 5) icon.Padding = 30;
            }
        }

        private void OnPaddingChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (App.CurrentProject == null)
                return;
            foreach (var icon in App.CurrentProject.Icons.Where(i => i.AppSelected)) icon.Padding = PaddingSlider.Value;
        }

        private void LeftRightChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (App.CurrentProject == null)
                return;
            foreach (var icon in App.CurrentProject.Icons.Where(i => i.AppSelected))
                icon.LeftRightOffset = LeftRightSlider.Value;
        }

        private void UpDownChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (App.CurrentProject == null)
                return;
            foreach (var icon in App.CurrentProject.Icons.Where(i => i.AppSelected))
                icon.UpDownOffset = UpDownSlider.Value;
        }

        private void SelectButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var icon in App.CurrentProject.Icons) icon.AppSelected = true;
        }

        private void DeSelectButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var icon in App.CurrentProject.Icons) icon.AppSelected = false;
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var icons = App.CurrentProject.Icons.Where(i => i.AppSelected).ToList();
            foreach (var icon in icons) App.CurrentProject.Icons.Remove(icon);
        }

        private async void AddClick(object sender, RoutedEventArgs e)
        {
            var addIconsDialog = new AddIconsDialog();
            await addIconsDialog.ShowAsync();
        }

        private void IsTopCommandBarClosing(object sender, object e)
        {
            TopCommandBar.IsOpen = true;
        }

        private void IsBottomCommandBarClosing(object sender, object e)
        {
            BottomCommandBar.IsOpen = true;
        }

        private void DisableBarButtons()
        {
            TopCommandBar.IsEnabled = false;
            BottomCommandBar.IsEnabled = false;
        }

        private void EnableBarButtons()
        {
            TopCommandBar.IsEnabled = true;
            BottomCommandBar.IsEnabled = true;
        }

        private async void SaveProjectClick(object sender, RoutedEventArgs e)
        {
            await IconSaver.Save();
        }

        private async void OpenProjectClick(object sender, RoutedEventArgs e)
        {
            var project = await IconSaver.LoadProject();
            if (project == null)
                return;
            IntroGrid.Visibility = Visibility.Collapsed;
            App.CurrentProject = project;
            IconsControl.ItemsSource = App.CurrentProject.Icons;
            EnableBarButtons();
            DeselectAll.IsEnabled = true;
            SelectAll.IsEnabled = true;
            AddIcons.IsEnabled = true;
            RemoveSelected.IsEnabled = true;
            ExportProject.IsEnabled = true;
            SaveProjectAs.IsEnabled = true;
            SaveProject.IsEnabled = true;
        }

        private async void SaveProjectAsClick(object sender, RoutedEventArgs e)
        {
            await IconSaver.Save(true);
        }
    }
}