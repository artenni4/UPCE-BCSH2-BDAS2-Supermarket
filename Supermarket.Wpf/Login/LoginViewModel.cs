using Supermarket.Core.Employees;
using Supermarket.Infrastructure.Employees;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.Login
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand EmployeeLoginCommand { get; set; }
        public ICommand CustomerLoginCommand { get; set; }
        private readonly IEmployeeService _employeeService;

        public LoginViewModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;

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
                var userId = await _employeeService.LoginEmployeeAsync(loginData);
            }
            // authorization
            // kakaja pokladna
        }

        private void CustomerLogin(object? obj)
        {
            // goto unauth view
        }

        private bool CanLogin(object? arg) { return true; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
