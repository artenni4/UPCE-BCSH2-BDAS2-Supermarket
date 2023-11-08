using System;
using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase, IDialogViewModel<ApplicationView, ILoggedEmployee>
    {
        public ICommand NavigateToCashboxCommand { get; }
        public ICommand NavigateToGoodsKeepingCommand { get; }

        public MenuViewModel(INavigationService navigationService)
        {
            NavigateToCashboxCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, ApplicationView.CashBox),
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            NavigateToGoodsKeepingCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, ApplicationView.Storage),
                _ => navigationService.CurrentView != ApplicationView.Storage);
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

        public event EventHandler<ApplicationView>? ResultReceived;
    }
}
