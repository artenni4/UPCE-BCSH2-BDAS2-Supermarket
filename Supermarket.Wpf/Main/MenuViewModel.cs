using System;
using System.Windows.Input;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase, IDialogViewModel<MenuResult>
    {
        public ICommand LogOutCommand { get; }
        public ICommand NavigateToCashboxCommand { get; }
        public ICommand NavigateToGoodsKeepingCommand { get; }
        public ICommand NavigateToManagerCommand { get; }

        public MenuViewModel(INavigationService navigationService, ILoggedUserService loggedUserService)
        {
            LoggedUserService = loggedUserService;
            
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

        private ILoggedUserService? _loggedUserService;
        public ILoggedUserService? LoggedUserService
        {
            get => _loggedUserService;
            private set => SetProperty(ref _loggedUserService, value);
        }

        public void SetParameters(EmptyParameters parameters) { }

        public event EventHandler<MenuResult>? ResultReceived;
    }
}
