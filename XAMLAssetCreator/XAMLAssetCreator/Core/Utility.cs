using Windows.UI;

namespace XAMLAssetCreator.Core
{
    public static class Utility
    {
        public static string HexConverter(Color c)
        {
            return "#" +c.A.ToString("X2") +  c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}