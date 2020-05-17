using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class AddIconsDialog : ContentDialog
    {
        public AddIconsDialog()
        {
            ResourceMap.Initialise();
            Categories = ResourceMap.CategoryDict.Keys.ToList();
            Categories.Insert(0, "All");
            SelectedItems = new ObservableCollection<IconData>();
            Loaded += OnThisPageLoaded;
            SelectItemCommand = new RelayCommand<IconData>(OnItemCommand);
            InitializeComponent();
        }

        public ObservableCollection<IconData> SelectedItems { get; }
        public List<string> Categories { get; }
        public ICommand SelectItemCommand { get; }

        private void OnThisPageLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedIndex = 0;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                return;
            SetupIcons();
        }

        private void OnItemCommand(IconData icon)
        {
            SelectedItems.Add(icon);
            SelectedItemsListView.UpdateLayout();
            SelectedItemsListView.ScrollIntoView(icon);
        }

        private void SelectedItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedItems.Remove((IconData) e.ClickedItem);
        }

        private string GetUniqueName(string name, int counter = 0)
        {
            var retName = name;
            if (counter > 0) retName += counter.ToString();
            if (App.CurrentProject?.Icons?.All(i =>
                string.Compare(retName, i.Name, StringComparison.CurrentCultureIgnoreCase) != 0) == true)
                return retName;
            counter++;
            return GetUniqueName(name, counter);
        }

        private void OnComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                SearchText.Text = string.Empty;
            SetupIcons();
        }

        public void SetupIcons()
        {
            Repeater.ItemsSource = GetData();
        }

        private List<IconWrapper> GetData()
        {
            string category = ComboBox.SelectedItem as string;
            if (category == null || category != "All" && !ResourceMap.CategoryDict.ContainsKey(category))
                return null;
            List<IconWrapper> data;
            if (category == "All")
                data = ResourceMap.CategoryDict
                    .Values
                    .ToList()
                    .SelectMany(x => x)
                    .Select(x => new IconWrapper {Data = x, SelectCommand = SelectItemCommand})
                    .ToList();
            else
                data = ResourceMap.CategoryDict[category]
                    .Select(x => new IconWrapper {Data = x, SelectCommand = SelectItemCommand})
                    .ToList();
            return data.Where(c => c.Data.SearchText.ToLower().Contains(SearchText.Text.Trim())).ToList();
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            foreach (var item in SelectedItems)
            {
                var icon = new ProjectIcon
                {
                    BackType = BackgroundType.None,
                    BackgroundColor = Constants.TransparentColor,
                    ForegroundColor = Constants.AppAccentColor,
                    IconData = item.Data,
                    Padding = 5,
                    AppSelected = true,
                    Name = GetUniqueName("Icon")
                };
                App.CurrentProject.Icons.Add(icon);
            }

            SelectedItems.Clear();
        }
    }
}