using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using SkiaSharp;
using XAMLAssetCreator.Controls;

namespace XAMLAssetCreator.Core
{
    public static class IconExporter
    {
        public static async Task Export(StorageFolder mainFolder)
        {
            var project = App.CurrentProject;
            Dictionary<TargetFolder, StorageFolder> folders = null;
            try
            {
                folders = await GetFoldersAsync(mainFolder);
            }
            catch
            {
                var dialog =
                    new MessageDialog(
                        "Error creating the folders for your project, please check that you have the necessary permissions to create files and folders in the chosen directory");
                await dialog.ShowAsync();
                return;
            }

            foreach (var icon in project.Icons)
            {
                foreach (var s in project.StandardSizes)
                    try
                    {
                        var folder = GetFolderForConfig(s, folders);
                        var name = $"{icon.Name}{s.Suffix}.png";
                        var size = s.BaseSize * s.Factor;
                        await SkiaView.CreateImageInstance(icon.IconData, size, icon.Padding, icon.BackgroundColor,
                            icon.ForegroundColor, icon.BackType, icon.LeftRightOffset, icon.UpDownOffset, folder, name);
                    }
                    catch
                    {
                    }

                foreach (var size in project.CustomSizes)
                {
                    if (project.IncludeCustomApple)
                    {
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size, icon.Padding, icon.BackgroundColor,
                            icon.ForegroundColor, icon.BackType, icon.LeftRightOffset, icon.UpDownOffset,
                            folders[TargetFolder.Apple], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 2, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Apple], $"{icon.Name}{size.Size}@2x.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 3, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Apple], $"{icon.Name}{size.Size}@3x.png");
                    }

                    if (project.IncludeCustomWindows)
                    {
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size, icon.Padding, icon.BackgroundColor,
                            icon.ForegroundColor, icon.BackType, icon.LeftRightOffset, icon.UpDownOffset,
                            folders[TargetFolder.Windows], $"{icon.Name}{size.Size}.scale-100.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 1.25, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Windows], $"{icon.Name}{size.Size}.scale-125.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 1.5, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Windows], $"{icon.Name}{size.Size}.scale-150.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 2, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Windows], $"{icon.Name}{size.Size}.scale-200.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 4, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Windows], $"{icon.Name}{size.Size}.scale-400.png");
                    }

                    if (project.IncludeCustomAndroid)
                    {
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 0.75, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Androidldpi], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size, icon.Padding, icon.BackgroundColor,
                            icon.ForegroundColor, icon.BackType, icon.LeftRightOffset, icon.UpDownOffset,
                            folders[TargetFolder.Androidmdpi], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 1.5, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Androidhdpi], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 2, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Androidxhdpi], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 3, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Androidxxhdpi], $"{icon.Name}{size.Size}.png");
                        await SkiaView.CreateImageInstance(icon.IconData, size.Size * 4, icon.Padding,
                            icon.BackgroundColor, icon.ForegroundColor, icon.BackType, icon.LeftRightOffset,
                            icon.UpDownOffset, folders[TargetFolder.Androidxxxhdpi], $"{icon.Name}{size.Size}.png");
                    }
                }

                var xamlFolder = folders[TargetFolder.Xaml];
                var xamlFileName = $"{icon.Name}.xaml";
                var padding = icon.Padding;
                var left = (padding + icon.LeftRightOffset)/2d;
                var right = (padding - icon.LeftRightOffset)/2d;
                var up = (padding + icon.UpDownOffset)/2d;
                var down = (padding - icon.UpDownOffset)/2d;
                var content = string.Empty;
                switch (icon.BackType)
                {
                    case BackgroundType.None:
                        content = $@"
<Viewbox Width=""200"" Height=""200"">
    <Grid Width=""100"" Height=""100"">
        <Path Stretch=""Fill"" Margin=""{left},{up},{right},{down}"" Data=""{icon.IconData}"" Fill=""{icon.ForegroundColor}"" />
    </Grid>
</Viewbox>";
                        break;
                    case BackgroundType.Square:
                        content = $@"
<Viewbox Width=""200"" Height=""200"">
    <Grid Width=""100"" Height=""100"">
        <Rectangle Width=""100"" Height=""100"" Fill=""{icon.BackgroundColor}"" />
        <Path Stretch=""Fill"" Margin=""{left},{up},{right},{down}"" Data=""{icon.IconData}"" Fill=""{icon.ForegroundColor}"" />
    </Grid>
</Viewbox>";
                        break;
                    case BackgroundType.Circle:
                        content = $@"
<Viewbox Width=""200"" Height=""200"">
    <Grid Width=""100"" Height=""100"">
        <Ellipse Width=""100"" Height=""100"" Fill=""{icon.BackgroundColor}"" />
        <Path Stretch=""Fill"" Margin=""{left},{up},{right},{down}"" Data=""{icon.IconData}"" Fill=""{icon.ForegroundColor}"" />
    </Grid>
</Viewbox>";
                        break;
                }
                var file = await xamlFolder.CreateFileAsync(xamlFileName, CreationCollisionOption.ReplaceExisting);
                using (var writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
                {
                    await writer.WriteLineAsync(content);
                }
            }
        }

        private static StorageFolder GetFolderForConfig(IconExportOption iconoption,
            Dictionary<TargetFolder, StorageFolder> folders)
        {
            switch (iconoption.SizeType)
            {
                case OutputPlatform.AppleIos:
                    return folders[TargetFolder.Apple];
                case OutputPlatform.AppleMac:
                    return folders[TargetFolder.Apple];
                case OutputPlatform.Windows:
                    return folders[TargetFolder.Windows];
                case OutputPlatform.AndroidLdpi:
                    return folders[TargetFolder.Androidldpi];
                case OutputPlatform.AndroidMdpi:
                    return folders[TargetFolder.Androidmdpi];
                case OutputPlatform.AndriodHdpi:
                    return folders[TargetFolder.Androidhdpi];
                case OutputPlatform.AndroidXhdpi:
                    return folders[TargetFolder.Androidxhdpi];
                case OutputPlatform.AndroidXxhdpi:
                    return folders[TargetFolder.Androidxxhdpi];
                case OutputPlatform.AndroidXxxhdpi:
                    return folders[TargetFolder.Androidxxxhdpi];
            }

            return null;
        }


        private static async Task<Dictionary<TargetFolder, StorageFolder>> GetFoldersAsync(StorageFolder folder)
        {
            var project = App.CurrentProject;
            var mainFolder = await folder.CreateFolderAsync(project.Name, CreationCollisionOption.ReplaceExisting);
            var folderIos = await mainFolder.CreateFolderAsync("Apple", CreationCollisionOption.ReplaceExisting);
            var folderWindows = await mainFolder.CreateFolderAsync("Windows", CreationCollisionOption.ReplaceExisting);
            var folderAndroid = await mainFolder.CreateFolderAsync("Android", CreationCollisionOption.ReplaceExisting);
            var folderXaml = await mainFolder.CreateFolderAsync("XAML", CreationCollisionOption.ReplaceExisting);
            var folderAldpi = await folderAndroid.CreateFolderAsync("ldpi", CreationCollisionOption.ReplaceExisting);
            var folderAmdpi = await folderAndroid.CreateFolderAsync("mdpi", CreationCollisionOption.ReplaceExisting);
            var folderAhdpi = await folderAndroid.CreateFolderAsync("hdpi", CreationCollisionOption.ReplaceExisting);
            var folderAxhdpi = await folderAndroid.CreateFolderAsync("xhdpi", CreationCollisionOption.ReplaceExisting);
            var folderAxxhdpi =
                await folderAndroid.CreateFolderAsync("xxhdpi", CreationCollisionOption.ReplaceExisting);
            var folderAxxxhdpi =
                await folderAndroid.CreateFolderAsync("xxxhdpi", CreationCollisionOption.ReplaceExisting);
            var dict = new Dictionary<TargetFolder, StorageFolder>
            {
                [TargetFolder.Apple] = folderIos, [TargetFolder.Windows] = folderWindows, [TargetFolder.Xaml] = folderXaml,
                [TargetFolder.Androidldpi] = folderAldpi, [TargetFolder.Androidmdpi] = folderAmdpi,
                [TargetFolder.Androidhdpi] = folderAhdpi, [TargetFolder.Androidxhdpi] = folderAxhdpi,
                [TargetFolder.Androidxxhdpi] = folderAxxhdpi, [TargetFolder.Androidxxxhdpi] = folderAxxxhdpi
            };
            return dict;
        }

        private enum TargetFolder
        {
            Apple,
            Windows,
            Androidldpi,
            Androidmdpi,
            Androidhdpi,
            Androidxhdpi,
            Androidxxhdpi,
            Androidxxxhdpi,
            Xaml
        }
    }
}