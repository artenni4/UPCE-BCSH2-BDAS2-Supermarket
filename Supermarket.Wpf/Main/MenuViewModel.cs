using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase
    {
        private readonly ILoggedUserService _loggedUserService;

        public ICommand ToggleMenuCommand { get; }

        public ICommand NavigateToCashboxCommand { get; }

        public MenuViewModel(INavigationService navigationService, ILoggedUserService loggedUserService)
        {
            _loggedUserService = loggedUserService;

            ToggleMenuCommand = new RelayCommand(ToggleMenu, CanToggleMenu);

            NavigateToCashboxCommand = new RelayCommand(
                _ => { navigationService.NavigateTo(ApplicationView.CashBox); IsMenuVisible = false; },
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            _loggedUserService.EmployeeLoggedIn += EmployeeLoggedIn;
            _loggedUserService.EmployeeLoggedOut += EmployeeLoggedOut;
            LoggedEmployee = _loggedUserService.LoggedEmployee;
        }

        private void EmployeeLoggedOut(object? sender, System.EventArgs e)
        {
            LoggedEmployee = null;
        }

        private void EmployeeLoggedIn(object? sender, LoggedEmployeeArgs e)
        {
            LoggedEmployee = e.LoggedEmployee;
            IsMenuVisible = true;
        }

        private ILoggedEmployee? loggedEmployee;
        public ILoggedEmployee? LoggedEmployee
        {
            get => loggedEmployee;
            private set => SetProperty(ref loggedEmployee, value);
        }

        private bool isMenuVisible;
        public bool IsMenuVisible
        {
            get => isMenuVisible;
            private set => SetProperty(ref isMenuVisible, value);
        }

        private void ToggleMenu(object? arg)
        {
            IsMenuVisible = !IsMenuVisible;
        }

        private bool CanToggleMenu(object? arg)
        {
            if (!IsMenuVisible && LoggedEmployee is null)
            {
                return false;
            }
            return true;
        }
    }
}
