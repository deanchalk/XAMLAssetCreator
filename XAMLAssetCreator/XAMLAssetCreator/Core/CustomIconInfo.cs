namespace XAMLAssetCreator.Core
{
    public class CustomIconInfo : NotifyBase
    {
        private double _size;

        public double Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
    }
}