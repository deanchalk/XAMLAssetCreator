using XAMLAssetCreator.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class AddIcons : UserControl
    {
        public event EventHandler Cancel;
        public event EventHandler AddIconsDone;
        public AddIcons()
        {
            ResourceMap.Initialise();
            Categories = ResourceMap.CategoryDict.Keys.ToList();
            Categories.Insert(0, "All");
            SelectedItems = new ObservableCollection<IconData>();
            Loaded += OnThisPageLoaded;
            InitializeComponent();
        }

        private void SelectedItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedItems.Remove((IconData)e.ClickedItem);
        }

        private void OnThisPageLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedIndex = 0;
        }

        public ObservableCollection<IconData> SelectedItems { get; }
        public List<string> Categories { get; }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        private void AddIconClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in SelectedItems)
            {
                var icon = new ProjectIcon
                {
                    BackType = BackgroundType.None,
                    BackgroundColor = ColorPicker.Transparent,
                    ForegroundColor = "#B10821",
                    IconData = item.Data,
                    Padding = 5,
                    AppSelected = true,
                    Name = GetUniqueName("Icon")
                };
                App.CurrentProject.Icons.Add(icon);
            }
            
            SelectedItems.Clear();
            AddIconsDone?.Invoke(this, EventArgs.Empty);
        }

        private string GetUniqueName(string name, int counter = 0)
        {
            var retName = name;
            if (counter > 0)
            {
                retName += counter.ToString();
            }
            if (App.CurrentProject.Icons.All(i => string.Compare(retName, i.Name, StringComparison.CurrentCultureIgnoreCase) != 0))
            {
                return retName;
            }
            counter++;
            return GetUniqueName(name, counter);
        }

        private void OnComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                SearchText.Text = string.Empty;
            ListView.ItemsSource = GetData();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                return;
            var data = GetData(); ;
            ListView.ItemsSource = data;
        }

        private List<IconData> GetData()
        {
            var category = ComboBox.SelectedItem as string;
            if (category == null || (category != "All" && !ResourceMap.CategoryDict.ContainsKey(category)))
                return null;
            List<IconData> data;
            if (category == "All")
            {
                data = ResourceMap.CategoryDict
                    .Values
                    .ToList()
                    .SelectMany(x => x)
                    .ToList();
            }
            else
                data = ResourceMap.CategoryDict[category]
                    .ToList();
            return data.Where(c => c.SearchText.ToLower().Contains(SearchText.Text.Trim())).ToList();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedIcon = (IconData)e.ClickedItem;
            SelectedItems.Add(selectedIcon);
        }
    }
}