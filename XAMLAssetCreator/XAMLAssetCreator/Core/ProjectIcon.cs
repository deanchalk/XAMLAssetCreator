namespace XAMLAssetCreator.Core
{
    public class ProjectIcon : NotifyBase
    {
        private bool _appSelected;
        private string _backgroundColor;
        private BackgroundType _backType;
        private string _foregroundColor;
        private double _leftRightOffset;
        private string _name;
        private double _padding;
        private double _upDownOffset;
        public string IconData { get; set; }

        public double Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                OnPropertyChanged();
            }
        }

        public BackgroundType BackType
        {
            get => _backType;
            set
            {
                _backType = value;
                OnPropertyChanged();
            }
        }

        public string ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged();
            }
        }

        public string BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public bool AppSelected
        {
            get => _appSelected;
            set
            {
                _appSelected = value;
                OnPropertyChanged();
            }
        }

        public double LeftRightOffset
        {
            get => _leftRightOffset;
            set
            {
                _leftRightOffset = value;
                OnPropertyChanged();
            }
        }

        public double UpDownOffset
        {
            get => _upDownOffset;
            set
            {
                _upDownOffset = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}