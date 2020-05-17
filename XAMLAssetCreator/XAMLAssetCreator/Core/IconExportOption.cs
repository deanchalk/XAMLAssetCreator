using System.Collections.Generic;

namespace XAMLAssetCreator.Core
{
    public class IconExportOption : NotifyBase
    {
        private bool _isSelected;

        private IconExportOption(StandardIconConfig config, string platform, OutputPlatform sizeType, string label,
            string suffix, double baseSize, double factor, string description)
        {
            IconConfig = config;
            Label = label;
            Description = description;
            Suffix = suffix;
            Platform = platform;
            Factor = factor;
            BaseSize = baseSize;
            SizeType = sizeType;
            FileName = GetFileName(suffix, sizeType, baseSize);
            var size = baseSize * factor;
            ActualSize = $"{size}x{size}";
            DipSize = $"{baseSize}x{baseSize}";
            IsSelected = true;
        }

        public string Label { get; set; }
        public string Description { get; set; }
        public string Suffix { get; set; }
        public string Platform { get; set; }
        public double Factor { get; set; }
        public double BaseSize { get; set; }
        public OutputPlatform SizeType { get; set; }
        public string FileName { get; }
        public string ActualSize { get; }
        public string DipSize { get; }
        public StandardIconConfig IconConfig { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public static List<IconExportOption> GetOptions()
        {
            var icons = new List<IconExportOption>
            {
                new IconExportOption(StandardIconConfig.iOS29x1, "iPad", OutputPlatform.AppleIos, "iPad Settings",
                    "29", 29, 1, "iPad non-Retina settings"),
                new IconExportOption(StandardIconConfig.iOS29x2, "iPhone / iPad / Apple Watch",
                    OutputPlatform.AppleIos, "iPhone, iPad & Watch Settings", "29@2x", 29, 2,
                    "iPhone / iPad Retina settings and watch Retina companion settings"),
                new IconExportOption(StandardIconConfig.iOS29x3, "iPhone / Apple Watch", OutputPlatform.AppleIos,
                    "iPhone & Watch Settings", "29@3x", 29, 3,
                    "iPhone HD Retina settings and watch HD Retina companion settings"),
                new IconExportOption(StandardIconConfig.iOS40x1, "iPad", OutputPlatform.AppleIos,
                    "iPad Spotlight",
                    "40", 40, 1, "iPad non-Retina spotlight"),
                new IconExportOption(StandardIconConfig.iOS40x2, "iPhone / Apple Watch", OutputPlatform.AppleIos,
                    "iPhone / iPad Spotlight & Watch Home Screen", "40@2x", 40, 2,
                    "iPhone / iPad Retina spotlight and Watch home screen (all watch sizes)"),
                new IconExportOption(StandardIconConfig.iOS40x3, "iPhone", OutputPlatform.AppleIos,
                    "iPhone Spotlight", "40@3x", 40, 3, "Iphone HD Retina spotlight"),
                new IconExportOption(StandardIconConfig.iOS60x2, "iPhone", OutputPlatform.AppleIos, "iPhone App",
                    "60@2x", 60, 2, "Iphone Retina app"),
                new IconExportOption(StandardIconConfig.iOS60x3, "iPhone", OutputPlatform.AppleIos, "iPhone App",
                    "60@3x", 60, 3, "Iphone HD Retina app"),
                new IconExportOption(StandardIconConfig.iOS76x1, "iPad", OutputPlatform.AppleIos, "iPad App",
                    "76",
                    76, 1, "iPad non-Retina app"),
                new IconExportOption(StandardIconConfig.iOS76x2, "iPad", OutputPlatform.AppleIos, "iPad App",
                    "76@2x", 76, 2, "iPad Retina app"),
                new IconExportOption(StandardIconConfig.iOS83x2, "iPad Pro", OutputPlatform.AppleIos,
                    "iPad Pro App", "83@2x", 83.5, 2, "iPad Pro Retina app"),
                new IconExportOption(StandardIconConfig.iOS24x2, "Apple Watch", OutputPlatform.AppleIos,
                    "Watch Notifications", "24@2x", 24, 2, "Apple Watch notifications (38mm)"),
                new IconExportOption(StandardIconConfig.iOS27x2, "Apple Watch", OutputPlatform.AppleIos,
                    "Watch Notifications", "27@2x", 27.5, 2, "Apple Watch notifications (42mm)"),
                new IconExportOption(StandardIconConfig.iOS86x2, "Apple Watch", OutputPlatform.AppleIos,
                    "Watch Short Look", "86@2x", 86, 2, "Apple Watch short look (38mm)"),
                new IconExportOption(StandardIconConfig.iOS92x2, "Apple Watch", OutputPlatform.AppleIos,
                    "Watch Short Look", "98@2x", 98, 2, "Apple Watch short look (42mm)"),
                new IconExportOption(StandardIconConfig.iOS16x1, "Apple Mac", OutputPlatform.AppleMac, "Mac 16pt",
                    "16", 16, 1, "Mac 16pt"),
                new IconExportOption(StandardIconConfig.iOS16x2, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac Retina 16pt", "16@2x", 16, 2, "Mac Retina 16pt"),
                new IconExportOption(StandardIconConfig.iOS32x1, "Apple Mac", OutputPlatform.AppleMac, "Mac 32pt",
                    "32", 32, 1, "Mac 32pt"),
                new IconExportOption(StandardIconConfig.iOS32x2, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac Retina 32pt", "32@2x", 32, 2, "Mac Retina 32pt"),
                new IconExportOption(StandardIconConfig.iOS128x1, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac 128pt", "128", 128, 1, "Mac 128pt"),
                new IconExportOption(StandardIconConfig.iOS128x2, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac Retina 128pt", "128@2x", 128, 2, "Mac Retina 128pt"),
                new IconExportOption(StandardIconConfig.iOS256x1, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac 256pt", "256", 256, 1, "Mac 256pt"),
                new IconExportOption(StandardIconConfig.iOS256x2, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac Retina 256pt", "256@2x", 256, 2, "Mac Retina 256pt"),
                new IconExportOption(StandardIconConfig.iOS512x1, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac 512pt", "512", 512, 1, "Mac 512pt"),
                new IconExportOption(StandardIconConfig.iOS512x2, "Apple Mac", OutputPlatform.AppleMac,
                    "Mac Retina 512pt", "512@2x", 512, 2, "Mac Retina 512pt"),
                new IconExportOption(StandardIconConfig.Win71x100, "Windows 10", OutputPlatform.Windows,
                    "Win Small Tile 71x71 100%", "71.scale-100", 71, 1, "Windows 10 Small Tile Logo 71x71 @ 100%"),
                new IconExportOption(StandardIconConfig.Win71x125, "Windows 10", OutputPlatform.Windows,
                    "Win Small Tile 71x71 125%", "71.scale-125", 89, 1, "Windows 10 Small Tile Logo 71x71 @ 125%"),
                new IconExportOption(StandardIconConfig.Win71x150, "Windows 10", OutputPlatform.Windows,
                    "Win Small Tile 71x71 150%", "71.scale-150", 107, 1, "Windows 10 Small Tile Logo 71x71 @ 150%"),
                new IconExportOption(StandardIconConfig.Win71x200, "Windows 10", OutputPlatform.Windows,
                    "Win Small Tile 71x71 200%", "71.scale-200", 71, 2, "Windows 10 Small Tile Logo 71x71 @ 200%"),
                new IconExportOption(StandardIconConfig.Win71x400, "Windows 10", OutputPlatform.Windows,
                    "Win Small Tile 71x71 400%", "71.scale-400", 71, 4, "Windows 10 Small Tile Logo 71x71 @ 400%"),
                new IconExportOption(StandardIconConfig.Win150x100, "Windows 10", OutputPlatform.Windows,
                    "Win Medium Tile 150x150 100%", "150.scale-100", 150, 1,
                    "Windows 10 Medium Tile Logo 150x150 @ 100%"),
                new IconExportOption(StandardIconConfig.Win150x125, "Windows 10", OutputPlatform.Windows,
                    "Win Medium Tile 150x150 125%", "150.scale-125", 188, 1,
                    "Windows 10 Medium Tile Logo 150x150 @ 125%"),
                new IconExportOption(StandardIconConfig.Win150x150, "Windows 10", OutputPlatform.Windows,
                    "Win Medium Tile 150x150 150%", "150.scale-150", 225, 1,
                    "Windows 10 Medium Tile Logo 150x150 @ 150%"),
                new IconExportOption(StandardIconConfig.Win150x200, "Windows 10", OutputPlatform.Windows,
                    "Win Medium Tile 150x150 200%", "150.scale-200", 150, 2,
                    "Windows 10 Medium Tile Logo 150x150 @ 200%"),
                new IconExportOption(StandardIconConfig.Win150x400, "Windows 10", OutputPlatform.Windows,
                    "Win Medium Tile 150x150 400%", "150.scale-400", 150, 4,
                    "Windows 10 Medium Tile Logo 150x150 @ 400%"),
                new IconExportOption(StandardIconConfig.Win310x100, "Windows 10", OutputPlatform.Windows,
                    "Win Large Tile 310x310 100%", "310.scale-100", 310, 1,
                    "Windows 10 Large Tile Logo 310x310 @ 100%"),
                new IconExportOption(StandardIconConfig.Win310x125, "Windows 10", OutputPlatform.Windows,
                    "Win Large Tile 310x310 125%", "310.scale-125", 388, 1,
                    "Windows 10 Large Tile Logo 310x310 @ 125%"),
                new IconExportOption(StandardIconConfig.Win310x150, "Windows 10", OutputPlatform.Windows,
                    "Win Large Tile 310x310 150%", "310.scale-150", 465, 1,
                    "Windows 10 Large Tile Logo 310x310 @ 150%"),
                new IconExportOption(StandardIconConfig.Win310x200, "Windows 10", OutputPlatform.Windows,
                    "Win Large Tile 310x310 200%", "310.scale-200", 310, 2,
                    "Windows 10 Large Tile Logo 310x310 @ 200%"),
                new IconExportOption(StandardIconConfig.Win310x400, "Windows 10", OutputPlatform.Windows,
                    "Win Large Tile 310x310 400%", "310.scale-400", 310, 4,
                    "Windows 10 Large Tile Logo 310x310 @ 400%"),
                new IconExportOption(StandardIconConfig.Win44x100, "Windows 10", OutputPlatform.Windows,
                    "Win App List 44x44 100%", "44.scale-100", 44, 1, "Windows 10 App List Logo 44x44 @ 100%"),
                new IconExportOption(StandardIconConfig.Win44x125, "Windows 10", OutputPlatform.Windows,
                    "Win App List 44x44 125%", "44.scale-125", 55, 1, "Windows 10 App List Logo 44x44 @ 125%"),
                new IconExportOption(StandardIconConfig.Win44x150, "Windows 10", OutputPlatform.Windows,
                    "Win App List 44x44 150%", "44.scale-150", 66, 1, "Windows 10 App List Logo 44x44 @ 150%"),
                new IconExportOption(StandardIconConfig.Win44x200, "Windows 10", OutputPlatform.Windows,
                    "Win App List 44x44 200%", "44.scale-200", 44, 2, "Windows 10 App List Logo 44x44 @ 200%"),
                new IconExportOption(StandardIconConfig.Win44x400, "Windows 10", OutputPlatform.Windows,
                    "Win App List 44x44 400%", "44.scale-400", 44, 4, "Windows 10 App List Logo 44x44 @ 400%"),
                new IconExportOption(StandardIconConfig.Win50x100, "Windows 10", OutputPlatform.Windows,
                    "Win Store 50x50 100%", "50.scale-100", 50, 1, "Windows 10 Store Logo 50x50 @ 100%"),
                new IconExportOption(StandardIconConfig.Win50x125, "Windows 10", OutputPlatform.Windows,
                    "Win Store 50x50 125%", "50.scale-125", 63, 1, "Windows 10 Store Logo 50x50 @ 125%"),
                new IconExportOption(StandardIconConfig.Win50x150, "Windows 10", OutputPlatform.Windows,
                    "Win Store 50x50 150%", "50.scale-150", 50, 1.5, "Windows 10 Store Logo 50x50 @ 150%"),
                new IconExportOption(StandardIconConfig.Win50x200, "Windows 10", OutputPlatform.Windows,
                    "Win Store 50x50 200%", "50.scale-200", 50, 2, "Windows 10 Store Logo 50x50 @ 200%"),
                new IconExportOption(StandardIconConfig.Win50x400, "Windows 10", OutputPlatform.Windows,
                    "Win Store 50x50 400%", "50.scale-400", 50, 4, "Windows 10 Store Logo 50x50 @ 400%"),
                new IconExportOption(StandardIconConfig.Win24x100, "Windows 10", OutputPlatform.Windows,
                    "Win Store 24x24 100%", "24.scale-100", 24, 1, "Windows 10 Badge Logo 24x24 @ 100%"),
                new IconExportOption(StandardIconConfig.Win24x125, "Windows 10", OutputPlatform.Windows,
                    "Win Store 24x24 125%", "24.scale-125", 24, 1.25, "Windows 10 Badge Logo 24x24 @ 125%"),
                new IconExportOption(StandardIconConfig.Win24x150, "Windows 10", OutputPlatform.Windows,
                    "Win Store 24x24 150%", "24.scale-150", 24, 1.5, "Windows 10 Badge Logo 24x24 @ 150%"),
                new IconExportOption(StandardIconConfig.Win24x200, "Windows 10", OutputPlatform.Windows,
                    "Win Store 24x24 200%", "24.scale-200", 24, 2, "Windows 10 Badge Logo 24x24 @ 200%"),
                new IconExportOption(StandardIconConfig.Win24x400, "Windows 10", OutputPlatform.Windows,
                    "Win Store 24x24 400%", "24.scale-400", 24, 4, "Windows 10 Badge Logo 24x24 @ 400%"),
                new IconExportOption(StandardIconConfig.Win16, "Windows 10", OutputPlatform.Windows,
                    "Win App List 16x16", ".targetsize-16", 16, 1, "Windows 10 App List Logo 16x16"),
                new IconExportOption(StandardIconConfig.Win24, "Windows 10", OutputPlatform.Windows,
                    "Win App List 24x24", ".targetsize-24", 24, 1, "Windows 10 App List Logo 24x24"),
                new IconExportOption(StandardIconConfig.Win32, "Windows 10", OutputPlatform.Windows,
                    "Win App List 32x32", ".targetsize-32", 32, 1, "Windows 10 App List Logo 32x32"),
                new IconExportOption(StandardIconConfig.Win48, "Windows 10", OutputPlatform.Windows,
                    "Win App List 48x48", ".targetsize-48", 48, 1, "Windows 10 App List Logo 48x48"),
                new IconExportOption(StandardIconConfig.Win256, "Windows 10", OutputPlatform.Windows,
                    "Win App List 256x256", ".targetsize-256", 256, 1, "Windows 10 App List Logo 256x256"),
                new IconExportOption(StandardIconConfig.Win20, "Windows 10", OutputPlatform.Windows,
                    "Win App List 20x20", ".targetsize-20", 20, 1, "Windows 10 App List Logo 20x20"),
                new IconExportOption(StandardIconConfig.Win30, "Windows 10", OutputPlatform.Windows,
                    "Win App List 30x30", ".targetsize-30", 30, 1, "Windows 10 App List Logo 30x30"),
                new IconExportOption(StandardIconConfig.Win36, "Windows 10", OutputPlatform.Windows,
                    "Win App List 36x36", ".targetsize-36", 36, 1, "Windows 10 App List Logo 36x36"),
                new IconExportOption(StandardIconConfig.Win40, "Windows 10", OutputPlatform.Windows,
                    "Win App List 40x40", ".targetsize-40", 40, 1, "Windows 10 App List Logo 40x40"),
                new IconExportOption(StandardIconConfig.Win60, "Windows 10", OutputPlatform.Windows,
                    "Win App List 60x60", ".targetsize-60", 60, 1, "Windows 10 App List Logo 60x60"),
                new IconExportOption(StandardIconConfig.Win64, "Windows 10", OutputPlatform.Windows,
                    "Win App List 64x64", ".targetsize-64", 64, 1, "Windows 10 App List Logo 64x64"),
                new IconExportOption(StandardIconConfig.Win72, "Windows 10", OutputPlatform.Windows,
                    "Win App List 72x72", ".targetsize-72", 72, 1, "Windows 10 App List Logo 72x72"),
                new IconExportOption(StandardIconConfig.Win80, "Windows 10", OutputPlatform.Windows,
                    "Win App List 80x80", ".targetsize-80", 80, 1, "Windows 10 App List Logo 80x80"),
                new IconExportOption(StandardIconConfig.Win96, "Windows 10", OutputPlatform.Windows,
                    "Win App List 96x96", ".targetsize-96", 96, 1, "Windows 10 App List Logo 96x96"),
                new IconExportOption(StandardIconConfig.And48ldpi, "Android", OutputPlatform.AndroidLdpi,
                    "Launcher Icon 48x48 ldpi", "48", 48, 0.75, "Android App Launcher Icon 48x48 - ldpi"),
                new IconExportOption(StandardIconConfig.And48mdpi, "Android", OutputPlatform.AndroidMdpi,
                    "Launcher Icon 48x48 mdpi", "48", 48, 1, "Android App Launcher Icon 48x48 - mdpi"),
                new IconExportOption(StandardIconConfig.And48hdpi, "Android", OutputPlatform.AndriodHdpi,
                    "Launcher Icon 48x48 hdpi", "48", 48, 1.5, "Android App Launcher Icon 48x48 - hdpi"),
                new IconExportOption(StandardIconConfig.And48xhdpi, "Android", OutputPlatform.AndroidXhdpi,
                    "Launcher Icon 48x48 xhdpi", "48", 48, 2, "Android App Launcher Icon 48x48 - xhdpi"),
                new IconExportOption(StandardIconConfig.And48xxhdpi, "Android", OutputPlatform.AndroidXxhdpi,
                    "Launcher Icon 48x48 xxhdpi", "48", 48, 3, "Android App Launcher Icon 48x48 - xxhdpi"),
                new IconExportOption(StandardIconConfig.And48xxxhdpi, "Android", OutputPlatform.AndroidXxxhdpi,
                    "Launcher Icon 48x48 xxxhdpi", "48", 48, 4, "Android App Launcher Icon 48x48 - xxxhdpi"),
                new IconExportOption(StandardIconConfig.And24ldpi, "Android", OutputPlatform.AndroidLdpi,
                    "Action / Notification Icon 24x24 ldpi", "24", 24, 0.75,
                    "Android Action Bar & Notification Icon 24x24 - ldpi"),
                new IconExportOption(StandardIconConfig.And24mdpi, "Android", OutputPlatform.AndroidMdpi,
                    "Action / Notification Icon 24x24 mdpi", "24", 24, 1,
                    "Android Action Bar & Notification Icon 24x24 - mdpi"),
                new IconExportOption(StandardIconConfig.And24hdpi, "Android", OutputPlatform.AndriodHdpi,
                    "Action / Notification Icon 24x24 hdpi", "24", 24, 1.5,
                    "Android Action Bar & Notification Icon 24x24 - hdpi"),
                new IconExportOption(StandardIconConfig.And24xhdpi, "Android", OutputPlatform.AndroidXhdpi,
                    "Action / Notification Icon 24x24 xhdpi", "24", 24, 2,
                    "Android Action Bar & Notification Icon 24x24 - xhdpi"),
                new IconExportOption(StandardIconConfig.And24xxhdpi, "Android", OutputPlatform.AndroidXxhdpi,
                    "Action / Notification Icon 24x24 xxhdpi", "24", 24, 3,
                    "Android Action Bar & Notification Icon 24x24 - xxhdpi"),
                new IconExportOption(StandardIconConfig.And24xxxhdpi, "Android", OutputPlatform.AndroidXxxhdpi,
                    "Action / Notification Icon 24x24 xxxhdpi", "24", 24, 4,
                    "Android Action Bar & Notification Icon 24x24 - xxxhdpi"),
                new IconExportOption(StandardIconConfig.And16ldpi, "Android", OutputPlatform.AndroidLdpi,
                    "Small Icon 16x16 ldpi", "16", 16, 0.75, "Android Small Icon 16x16 - ldpi"),
                new IconExportOption(StandardIconConfig.And16mdpi, "Android", OutputPlatform.AndroidMdpi,
                    "Small Icon 16x16 mdpi", "16", 16, 1, "Android Small Icon 16x16 - mdpi"),
                new IconExportOption(StandardIconConfig.And16hdpi, "Android", OutputPlatform.AndriodHdpi,
                    "Small Icon 16x16 hdpi", "16", 16, 1.5, "Android Small Icon 16x16- hdpi"),
                new IconExportOption(StandardIconConfig.And16xhdpi, "Android", OutputPlatform.AndroidXhdpi,
                    "Small Icon 16x16 xhdpi", "16", 16, 2, "Android Small Icon 16x16 - xhdpi"),
                new IconExportOption(StandardIconConfig.And16xxhdpi, "Android", OutputPlatform.AndroidXxhdpi,
                    "Small Icon 16x16 xxhdpi", "16", 16, 3, "Android Small Icon 16x16 - xxhdpi"),
                new IconExportOption(StandardIconConfig.And16xxxhdpi, "Android", OutputPlatform.AndroidXxxhdpi,
                    "Small Icon 16x16 xxxhdpi", "16", 16, 4, "Android Small Icon 16x16 - xxxhdpi")
            };


            return icons;
        }

        private string GetFileName(string suffix, OutputPlatform platform, double size)
        {
            var fileName = $"[name]{suffix}.png";
            switch (platform)
            {
                case OutputPlatform.AndroidLdpi:
                    return $"/lpdi/[name]{size}.png";
                case OutputPlatform.AndroidMdpi:
                    return $"/mpdi/[name]{size}.png";
                case OutputPlatform.AndriodHdpi:
                    return $"/hpdi/[name]{size}.png";
                case OutputPlatform.AndroidXhdpi:
                    return $"/xhpdi/[name]{size}.png";
                case OutputPlatform.AndroidXxhdpi:
                    return $"/xxhpdi/[name]{size}.png";
                case OutputPlatform.AndroidXxxhdpi:
                    return $"/xxxhpdi/[name]{size}.png";
                default:
                    return fileName;
            }
        }
    }

    public enum StandardIconConfig
    {
        iOS29x1 = 0,
        iOS29x2 = 1,
        iOS29x3 = 2,
        iOS40x1 = 86,
        iOS40x2 = 3,
        iOS40x3 = 4,
        iOS60x2 = 5,
        iOS60x3 = 6,
        iOS76x1 = 7,
        iOS76x2 = 8,
        iOS83x2 = 9,
        iOS24x2 = 10,
        iOS27x2 = 11,
        iOS86x2 = 12,
        iOS92x2 = 13,
        iOS16x1 = 14,
        iOS16x2 = 15,
        iOS32x1 = 16,
        iOS32x2 = 17,
        iOS128x1 = 18,
        iOS128x2 = 19,
        iOS256x1 = 20,
        iOS256x2 = 21,
        iOS512x1 = 22,
        iOS512x2 = 23,
        Win71x100 = 24,
        Win71x125 = 25,
        Win71x150 = 26,
        Win71x200 = 27,
        Win71x400 = 28,
        Win150x100 = 29,
        Win150x125 = 30,
        Win150x150 = 31,
        Win150x200 = 32,
        Win150x400 = 33,
        Win310x100 = 34,
        Win310x125 = 35,
        Win310x150 = 36,
        Win310x200 = 37,
        Win310x400 = 38,
        Win44x100 = 39,
        Win44x125 = 40,
        Win44x150 = 41,
        Win44x200 = 42,
        Win44x400 = 43,
        Win16 = 44,
        Win24 = 45,
        Win32 = 46,
        Win48 = 47,
        Win256 = 48,
        Win20 = 49,
        Win30 = 50,
        Win36 = 51,
        Win40 = 52,
        Win60 = 53,
        Win64 = 54,
        Win72 = 55,
        Win80 = 56,
        Win96 = 57,
        And48ldpi = 58,
        And48mdpi = 59,
        And48hdpi = 60,
        And48xhdpi = 61,
        And48xxhdpi = 62,
        And48xxxhdpi = 63,
        And24ldpi = 64,
        And24mdpi = 65,
        And24hdpi = 66,
        And24xhdpi = 67,
        And24xxhdpi = 68,
        And24xxxhdpi = 69,
        And16ldpi = 70,
        And16mdpi = 71,
        And16hdpi = 72,
        And16xhdpi = 73,
        And16xxhdpi = 74,
        And16xxxhdpi = 75,
        Win50x100 = 76,
        Win50x125 = 77,
        Win50x150 = 78,
        Win50x200 = 79,
        Win50x400 = 80,
        Win24x100 = 81,
        Win24x125 = 82,
        Win24x150 = 83,
        Win24x200 = 84,
        Win24x400 = 85
    }


    public enum OutputPlatform
    {
        AppleIos = 0,
        AppleMac = 1,
        Windows = 2,
        AndroidLdpi = 3,
        AndroidMdpi = 4,
        AndriodHdpi = 5,
        AndroidXhdpi = 6,
        AndroidXxhdpi = 7,
        AndroidXxxhdpi = 8
    }
}