using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Core.UseCases.Login;
using Supermarket.Core.Domain.Auth;

namespace Supermarket.Wpf.Login
{
    internal class LoginViewModel : NotifyPropertyChangedBase, IViewModel
    {
        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        private readonly ILoggedUserService _sessionService;

        public ICommand EmployeeLoginCommand { get; }
        public ICommand CustomerLoginCommand { get; }

        public LoginViewModel(ILoginService loginService, INavigationService navigationService, ILoggedUserService sessionService)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            _sessionService = sessionService;

            EmployeeLoginCommand = new RelayCommand(EmployeeLoginAsync, CanEmployeeLogin);
            CustomerLoginCommand = new RelayCommand(CustomerLogin);
        }


        private LoginModel _employeeLoginData = new();
        public LoginModel EmployeeLoginData
        {
            get => _employeeLoginData;
            set => SetProperty(ref _employeeLoginData, value);
        }

        private async void EmployeeLoginAsync(object? obj)
        {
            if (string.IsNullOrEmpty(EmployeeLoginData.Login) || string.IsNullOrEmpty(EmployeeLoginData.Password))
            {
                return;
            }

            var loginData = new LoginData
            {
                Login = EmployeeLoginData.Login,
                Password = EmployeeLoginData.Password
            };

            try
            {
                var loggedEmployee = await _loginService.LoginEmployeeAsync(loginData);
                _sessionService.SetLoggedEmployee(loggedEmployee);
            }
            catch (InvalidCredentialsException)
            {
                MessageBox.Show("Špatné příhlašovací údaje", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CustomerLogin(object? obj)
        {
            await _navigationService.NavigateToAsync(ApplicationView.CashBox);
        }

        private bool CanEmployeeLogin(object? arg) => !string.IsNullOrWhiteSpace(EmployeeLoginData.Login) && !string.IsNullOrWhiteSpace(EmployeeLoginData.Password);
    }
}
