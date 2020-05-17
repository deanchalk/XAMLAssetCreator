using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;

namespace XAMLAssetCreator.Core
{
    public static class IconSaver
    {
        public static async Task Save(bool saveAs = false)
        {
            var doc = CreateDoc();
            if (App.CurrentFile == null || saveAs)
            {
                var picker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    CommitButtonText = "Save Icon Project",
                    DefaultFileExtension = ".icpro"
                };
                picker.FileTypeChoices["Icon Creator Pro Project"] = new List<string>(new[] {".icpro"});
                picker.SuggestedFileName = App.CurrentProject.Name + ".icpro";
                var file = await picker.PickSaveFileAsync();
                if (file == null)
                    return;
                App.CurrentFile = file;
            }

            CachedFileManager.DeferUpdates(App.CurrentFile);
            await FileIO.WriteTextAsync(App.CurrentFile, doc.ToString(SaveOptions.None));
            var status = await CachedFileManager.CompleteUpdatesAsync(App.CurrentFile);
            if (status == FileUpdateStatus.Complete)
                await new MessageDialog("Project was successfully saved").ShowAsync();
            else
                await new MessageDialog("Project could not be saved").ShowAsync();
        }

        public static async Task<Project> LoadProject()
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                CommitButtonText = "Open Icon Project"
            };
            filePicker.FileTypeFilter.Add(".icpro");

            var file = await filePicker.PickSingleFileAsync();
            if (file == null)
                return null;
            var text = await FileIO.ReadTextAsync(file);
            var doc = XDocument.Parse(text);
            App.CurrentFile = file;
            return LoadAndCreateProject(doc);
        }

        private static bool GetAttributeValueBool(XAttribute a)
        {
            var success = bool.TryParse(a?.Value, out var val);
            return success && val;
        }

        private static double GetAttributeValueDouble(XAttribute a)
        {
            var success = double.TryParse(a?.Value, out var val);
            return success ? val : 0d;
        }

        private static TEnum GetAttributeValueEnum<TEnum>(XAttribute a)
        {
            if (a == null) return default;
            return (TEnum) Enum.Parse(typeof(TEnum), a.Value);
        }

        private static Project LoadAndCreateProject(XDocument doc)
        {
            var project = new Project();
            foreach (var size in project.StandardSizes) size.IsSelected = false;
            var root = doc.Element("IconProject");
            if (root == null) return project;
            project.IncludeCustomAndroid = GetAttributeValueBool(root.Attribute("IncludeCustomAndroid"));
            project.IncludeCustomApple = GetAttributeValueBool(root.Attribute("IncludeCustomApple"));
            project.IncludeCustomWindows = GetAttributeValueBool(root.Attribute("IncludeCustomWindows"));
            project.Name = root.Attribute("Name")?.Value;
            foreach (var element in root.Element("Icons")?.Elements("Icon") ?? new XElement[] { })
            {
                var picon = new ProjectIcon
                {
                    Name = element.Attribute("Name")?.Value,
                    Padding = GetAttributeValueDouble(element.Attribute("Padding")),
                    BackType = GetAttributeValueEnum<BackgroundType>(element.Attribute("BackType")),
                    ForegroundColor = element.Attribute("ForegroundColor")?.Value,
                    BackgroundColor = element.Attribute("BackgroundColor")?.Value,
                    AppSelected = GetAttributeValueBool(element.Attribute("AppSelected")),
                    LeftRightOffset = GetAttributeValueDouble(element.Attribute("LeftRightOffset")),
                    UpDownOffset = GetAttributeValueDouble(element.Attribute("UpDownOffset")),
                    IconData = element.Value
                };
                project.Icons.Add(picon);
            }

            foreach (var custom in root.Element("CustomSizes")?.Elements("CustomSize") ?? new XElement[] { })
            {
                var cutomSize = new CustomIconInfo
                {
                    Size = GetAttributeValueDouble(custom.Attribute("Size"))
                };
                project.CustomSizes.Add(cutomSize);
            }

            foreach (var standard in root.Element("StandardSizes")?.Elements("StandardSize") ?? new XElement[] { })
            {
                var sizeInfo = GetAttributeValueEnum<StandardIconConfig>(standard.Attribute("SizeId"));
                project.StandardSizes.First(s => s.IconConfig == sizeInfo).IsSelected = true;
            }

            return project;
        }

        private static XDocument CreateDoc()
        {
            var doc = new XDocument();
            var project = App.CurrentProject;
            var root = new XElement("IconProject");
            doc.Add(root);
            root.Add(new XAttribute("Name", project.Name));
            root.Add(new XAttribute("IncludeCustomApple", project.IncludeCustomApple));
            root.Add(new XAttribute("IncludeCustomAndroid", project.IncludeCustomAndroid));
            root.Add(new XAttribute("IncludeCustomWindows", project.IncludeCustomWindows));
            var icons = new XElement("Icons");
            root.Add(icons);
            foreach (var icon in project.Icons)
            {
                var iconElement = new XElement("Icon");
                iconElement.Add(new XAttribute("Name", icon.Name));
                iconElement.Add(new XAttribute("Padding", icon.Padding));
                iconElement.Add(new XAttribute("BackType", icon.BackType));
                iconElement.Add(new XAttribute("ForegroundColor", icon.ForegroundColor));
                iconElement.Add(new XAttribute("BackgroundColor", icon.BackgroundColor));
                iconElement.Add(new XAttribute("AppSelected", icon.AppSelected));
                iconElement.Add(new XAttribute("LeftRightOffset", icon.LeftRightOffset));
                iconElement.Add(new XAttribute("UpDownOffset", icon.UpDownOffset));
                iconElement.Value = icon.IconData;
                icons.Add(iconElement);
            }

            var standard = new XElement("StandardSizes");
            root.Add(standard);
            foreach (var size in project.StandardSizes)
            {
                var standardElement = new XElement("StandardSize");
                standardElement.Add(new XAttribute("SizeId", size.IconConfig));
                standard.Add(standardElement);
            }

            var custom = new XElement("CustomSizes");
            root.Add(custom);
            foreach (var size in project.CustomSizes)
            {
                var customElement = new XElement("CustomSize");
                customElement.Add(new XAttribute("Size", size.Size));
                custom.Add(customElement);
            }

            return doc;
        }
    }
}