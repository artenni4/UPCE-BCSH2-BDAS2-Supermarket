using System;
using System.Diagnostics;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Menu;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Main
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        public MainViewModel(INavigationService navigationService, IViewModelResolver viewModelResolver, MenuViewModel menuViewModel)
        {
            navigationService.NavigationSucceeded += NavigationCompleted;

            viewModelResolver.InitializationStarted += (_, _) =>
            {
                IsProgressVisible = true;
                Debug.WriteLine($"{nameof(IsProgressVisible)} is set to true in {nameof(MainViewModel)}");
            };
            viewModelResolver.InitializationFinished += (_, _) =>
            {
                IsProgressVisible = false;
                Debug.WriteLine($"{nameof(IsProgressVisible)} is set to false in {nameof(MainViewModel)}");
            };
            
            MenuViewModel = menuViewModel;
        }

        private void NavigationCompleted(object? sender, NavigationEventArgs e)
        {
            CurrentViewModel = e.NewViewModel;
            Debug.WriteLine($"{nameof(CurrentViewModel)} in {nameof(MainViewModel)} is set new value due to navigation completion");
        }
        
        private bool isProgressVisible;
        public bool IsProgressVisible
        {
            get => isProgressVisible;
            private set => SetProperty(ref isProgressVisible, value);
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
