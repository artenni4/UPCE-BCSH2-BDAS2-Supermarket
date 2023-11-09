using System;
using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase, IDialogViewModel<MenuResult, ILoggedEmployee>
    {
        public ICommand LogOutCommand { get; }
        public ICommand NavigateToCashboxCommand { get; }
        public ICommand NavigateToGoodsKeepingCommand { get; }
        public ICommand NavigateToManagerCommand { get; }

        public MenuViewModel(INavigationService navigationService)
        {
            LogOutCommand = new RelayCommand(_ => ResultReceived?.Invoke(this, MenuResult.LogOut()));
            
            NavigateToCashboxCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, MenuResult.Navigate(ApplicationView.CashBox)),
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            NavigateToGoodsKeepingCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, MenuResult.Navigate(ApplicationView.Storage)),
                _ => navigationService.CurrentView != ApplicationView.Storage);

            NavigateToManagerCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, MenuResult.Navigate(ApplicationView.Manager)),
                _ => navigationService.CurrentView != ApplicationView.Manager);
        }

        private ILoggedEmployee? _loggedEmployee;
        public ILoggedEmployee? LoggedEmployee
        {
            get => _loggedEmployee;
            private set => SetProperty(ref _loggedEmployee, value);
        }

        public void SetParameters(ILoggedEmployee parameters)
        {
            LoggedEmployee = parameters;
        }

        public event EventHandler<MenuResult>? ResultReceived;
    }
}
