using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using XAMLAssetCreator.Core;

namespace XAMLAssetCreator.Controls
{
    public sealed partial class NewProjectDialog : ContentDialog
    {
        public NewProjectDialog()
        {
            InitializeComponent();
        }

        private void OnCancel(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void CreateProject(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var projectName = ProjectNameBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(projectName))
            {
                return;
            }
            var project = new Project
            {
                Name = projectName,
                IncludeCustomAndroid = true,
                IncludeCustomApple = true,
                IncludeCustomWindows = true
            };
            project.CustomSizes.Add(new CustomIconInfo {Size = 100});
            project.CustomSizes.Add(new CustomIconInfo {Size = 150});
            App.CurrentProject = project;
            App.CurrentFile = null;
        }
    }
}