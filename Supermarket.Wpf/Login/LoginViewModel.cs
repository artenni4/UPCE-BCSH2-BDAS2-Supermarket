using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Core.UseCases.Login;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.Login
{
    internal class LoginViewModel : NotifyPropertyChangedBase, IViewModel
    {
        private readonly ILoginService _loginService;
        private readonly ILoggedUserService _loggedUserService;

        public ICommand EmployeeLoginCommand { get; }
        public ICommand CustomerLoginCommand { get; }

        public LoginViewModel(ILoginService loginService, ILoggedUserService loggedUserService)
        {
            _loginService = loginService;
            _loggedUserService = loggedUserService;

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
                if (loggedEmployee is LoggedSupermarketEmployee loggedSupermarketEmployee)
                {
                    _loggedUserService.SetSupermarketEmployee(loggedSupermarketEmployee);
                }
                else if (loggedEmployee is LoggedAdmin loggedAdmin)
                {
                    _loggedUserService.SetAdmin(loggedAdmin, 1);
                }
            }
            catch (InvalidCredentialsException)
            {
                MessageBox.Show("Špatné příhlašovací údaje", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CustomerLogin(object? obj)
        {
            _loggedUserService.SetCustomer(1);
        }

        private bool CanEmployeeLogin(object? arg) => !string.IsNullOrWhiteSpace(EmployeeLoginData.Login) && !string.IsNullOrWhiteSpace(EmployeeLoginData.Password);
    }
}
