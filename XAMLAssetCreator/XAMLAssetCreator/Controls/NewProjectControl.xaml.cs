using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using XAMLAssetCreator.Core;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace XAMLAssetCreator.Controls
{
    public sealed partial class NewProjectControl : UserControl
    {
        public NewProjectControl()
        {
            ResourceMap.Initialise();
            Categories = ResourceMap.CategoryDict.Keys.ToList();
            Categories.Insert(0, "All");
            SelectedItems = new ObservableCollection<IconData>();
            Loaded += OnPageLoaded;
            SelectItemCommand = new RelayCommand<IconData>(OnItemCommand);
            InitializeComponent();
        }

        private void OnItemCommand(IconData icon)
        {
            SelectedItems.Add(icon);
        }

        public List<string> Categories { get; }
        public ObservableCollection<IconData> SelectedItems { get; }
        public event EventHandler Cancel;
        public event EventHandler<Project> NewProject;

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedIndex = 0;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedIcon = (IconData) e.ClickedItem;
            SelectedItems.Add(selectedIcon);
        }

        private void OnComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                SearchText.Text = string.Empty;
           // ListView.ItemsSource = GetData();
            Repeater.ItemsSource = GetData();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText.Text.Trim().Length < 3)
                return;
           // ListView.ItemsSource = GetData();
           Repeater.ItemsSource = GetData();
        }

        private List<IconWrapper> GetData()
        {
            var category = ComboBox.SelectedItem as string;
            if (category == null || (category != "All" && !ResourceMap.CategoryDict.ContainsKey(category)))
                return null;
            List<IconWrapper> data;
            if (category == "All")
            {
                data = ResourceMap.CategoryDict
                    .Values
                    .ToList()
                    .SelectMany(x => x)
                    .Select(x => new IconWrapper { Data = x, SelectCommand = SelectItemCommand})
                    .ToList();
            }
            else
                data = ResourceMap.CategoryDict[category]
                    .Select(x => new IconWrapper { Data = x, SelectCommand = SelectItemCommand})
                    .ToList();
            return data.Where(c => c.Data.SearchText.ToLower().Contains(SearchText.Text.Trim())).ToList();
        }

        private void SelectedItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedItems.Remove((IconData) e.ClickedItem);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        private async void CreateProjectClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProjectName.Text.Trim()))
            {
                var dialog = new MessageDialog("You must enter a project name");
                await dialog.ShowAsync();
                return;
            }
            if (SelectedItems.Count == 0)
            {
                var dialog = new MessageDialog("You must select at least one icon from the icon picker");
                await dialog.ShowAsync();
                return;
            }
            var project = new Project
            {
                Name = ProjectName.Text.Trim(),
                IncludeCustomAndroid = true,
                IncludeCustomApple = true,
                IncludeCustomWindows = true
            };
            foreach (var item in SelectedItems)
            {
                var icon = new ProjectIcon
                {
                    BackType = BackgroundType.None,
                    BackgroundColor = "#00FFFFFF",
                    ForegroundColor = "#B10821",
                    IconData = item.Data,
                    AppSelected = true
                };
                project.Icons.Add(icon);
            }
            project.CustomSizes.Add(new CustomIconInfo {Size = 100});
            project.CustomSizes.Add(new CustomIconInfo {Size = 150});

            App.CurrentProject = project;
            foreach (var icon in App.CurrentProject.Icons)
            {
                icon.Name = GetUniqueName("Icon");
            }
            App.CurrentFile = null;
            NewProject?.Invoke(this, project);
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

        public ICommand SelectItemCommand { get; private set; }
    }
}