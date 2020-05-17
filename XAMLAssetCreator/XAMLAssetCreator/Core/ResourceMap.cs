using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace XAMLAssetCreator.Core
{
    public static class ResourceMap
    {
        private static bool _isBuilt;

        public static Dictionary<string, List<IconData>> CategoryDict =
            new Dictionary<string, List<IconData>>();

        public static void Initialise()
        {
            if (_isBuilt)
                return;
            _isBuilt = true;
            BuildMap();
        }

        private static void BuildMap()
        {
            var icons = typeof(ResourceMap)
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream("XAMLAssetCreator.Resources.Icons.xml");
            var doc = XDocument.Load(icons);
            if (doc.Root == null)
                return;
            foreach (var element in doc.Root.Elements())
            {
                var category = element.Attribute("Category")?.Value;
                if (category == null) continue;
                var label = element.Attribute("Label")?.Value;
                var searchText = element.Attribute("Keywords")?.Value;
                var data = element.Attribute("Value")?.Value;
                if (!CategoryDict.ContainsKey(category)) CategoryDict[category] = new List<IconData>();
                CategoryDict[category].Add(new IconData {Name = label, Data = data, SearchText = searchText});
            }
        }
    }
}