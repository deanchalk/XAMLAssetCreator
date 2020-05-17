using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XAMLAssetCreator.Core
{
    public class Project
    {
        public Project()
        {
            Icons = new ObservableCollection<ProjectIcon>();
            CustomSizes = new ObservableCollection<CustomIconInfo>();
            StandardSizes = IconExportOption.GetOptions();
        }

        public string Name { get; set; }
        public ObservableCollection<ProjectIcon> Icons { get; set; }
        public bool IncludeCustomApple { get; set; }
        public bool IncludeCustomWindows { get; set; }
        public bool IncludeCustomAndroid { get; set; }
        public ObservableCollection<CustomIconInfo> CustomSizes { get; set; }
        public List<IconExportOption> StandardSizes { get; set; }
    }
}