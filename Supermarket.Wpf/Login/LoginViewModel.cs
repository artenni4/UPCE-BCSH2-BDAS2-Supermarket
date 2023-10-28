using Supermarket.Core.Login;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Auth;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Wpf.Login
{
    public class LoginViewModel : NotifyPropertyChangedBase
    {
        private readonly ILoginService _loginService;
        private readonly ICashBoxService _cashBoxService;
        private readonly INavigationService _navigationService;


        public ICommand EmployeeLoginCommand { get; }
        public ICommand CustomerLoginCommand { get; }

        public LoginViewModel(ILoginService loginService, INavigationService navigationService, ICashBoxService cashBoxService)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            _cashBoxService = cashBoxService;

            EmployeeLoginCommand = new RelayCommand(EmployeeLoginAsync, CanEmployeeLogin);
            CustomerLoginCommand = new RelayCommand(CustomerLogin);
        }


        private LoginModel employeeLoginData = new();
        public LoginModel EmployeeLoginData
        {
            get => employeeLoginData;
            set => SetProperty(ref employeeLoginData, value);
        }

        private async void EmployeeLoginAsync(object? obj)
        {
            var a = await _cashBoxService.GetProductsAsync(1, new RecordsRange
            {
                PageNumber = 1,
                PageSize = 10
            }, 1, null);
            
            if (employeeLoginData.Login != null && employeeLoginData.Password != null)
            {
                var loginData = new LoginData
                {
                    Login = employeeLoginData.Login,
                    Password = employeeLoginData.Password
                };
                var userId = await _loginService.LoginEmployeeAsync(loginData);
            }
            // authorization
            // kakaja pokladna
        }

        private void CustomerLogin(object? obj)
        {
            _navigationService.NavigateTo(NavigateWindow.CashBox);
        }

        private bool CanEmployeeLogin(object? arg) => !string.IsNullOrWhiteSpace(EmployeeLoginData.Login) && !string.IsNullOrWhiteSpace(EmployeeLoginData.Password);
    }
}
