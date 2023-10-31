using Supermarket.Wpf.Common;
using Supermarket.Wpf.Menu;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;

namespace Supermarket.Wpf.Main
{
    internal class MainViewModel : NotifyPropertyChangedBase
    {
        public MainViewModel(INavigationService navigationService, MenuViewModel menuViewModel)
        {
            navigationService.NavigationCompleted += NavigationCompleted;
            MenuViewModel = menuViewModel;
        }
        
        private void NavigationCompleted(object? sender, NavigationEventArgs e)
        {
            CurrentViewModel = e.NewViewModel;
        }

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private MenuViewModel? menuViewModel;
        public MenuViewModel? MenuViewModel
        {
            get => menuViewModel;
            set => SetProperty(ref menuViewModel, value);
        }
    }
}
