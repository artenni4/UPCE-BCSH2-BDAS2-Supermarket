using Supermarket.Core.Employees;
using Supermarket.Core.Login;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace Supermarket.Wpf.Login
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand EmployeeLoginCommand { get; set; }
        public ICommand CustomerLoginCommand { get; set; }
        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;

        public LoginViewModel(ILoginService loginService, INavigationService navigationService)
        {
            _loginService = loginService;
            _navigationService = navigationService;

            EmployeeLoginCommand = new RelayCommand(EmployeeLoginAsync, CanLogin);
            CustomerLoginCommand = new RelayCommand(CustomerLogin, CanLogin);
        }


        private LoginModel employeeLoginData = new();
        public LoginModel EmployeeLoginData
        {
            get { return employeeLoginData; }
            set 
            { 
                employeeLoginData = value;
                OnPropertyChanged(nameof(EmployeeLoginData));
            }
        }

        private async void EmployeeLoginAsync(object? obj)
        {
            if (employeeLoginData.Login != null && employeeLoginData.Password != null) 
            {
                LoginData loginData = new LoginData { Login = employeeLoginData.Login, Password = employeeLoginData.Password };        
                var userId = await _loginService.LoginEmployeeAsync(loginData);
            }
            // authorization
            // kakaja pokladna
        }

        private void CustomerLogin(object? obj)
        {
            _navigationService.NavigateTo(NavigateWindow.CashBox);
        }

        private bool CanLogin(object? arg) { return true; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
