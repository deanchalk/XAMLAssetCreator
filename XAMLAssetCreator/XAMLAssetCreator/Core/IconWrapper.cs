using System.Windows.Input;

namespace XAMLAssetCreator.Core
{
    public struct IconWrapper
    {
        public IconData Data { get; set; }
        public ICommand SelectCommand { get; set; }
    }
}