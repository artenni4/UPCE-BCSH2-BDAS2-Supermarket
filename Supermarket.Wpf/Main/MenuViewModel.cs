using System;
using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase, IDialogViewModel<ApplicationView>
    {
        private readonly ILoggedUserService _loggedUserService;

        public ICommand NavigateToCashboxCommand { get; }

        public MenuViewModel(INavigationService navigationService, ILoggedUserService loggedUserService)
        {
            _loggedUserService = loggedUserService;

            NavigateToCashboxCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, ApplicationView.CashBox),
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            LoggedEmployee = _loggedUserService.LoggedEmployee;
        }

        private ILoggedEmployee? _loggedEmployee;
        public ILoggedEmployee? LoggedEmployee
        {
            get => _loggedEmployee;
            private set => SetProperty(ref _loggedEmployee, value);
        }

        public event EventHandler<ApplicationView>? ResultReceived;
    }
}
