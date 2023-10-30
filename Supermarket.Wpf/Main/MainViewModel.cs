using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Main
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        public MainViewModel()
        {
        }

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
    }
}
