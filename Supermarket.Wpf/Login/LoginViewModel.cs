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

        public LoginViewModel()
        {
            EmployeeLoginCommand = new RelayCommand(EmployeeLogin, CanLogin);
            CustomerLoginCommand = new RelayCommand(CustomerLogin, CanLogin);

            employeeLoginData = new();
        }

        private LoginModel employeeLoginData;
        public LoginModel EmployeeLoginData
        {
            get { return employeeLoginData; }
            set 
            { 
                employeeLoginData = value;
                OnPropertyChanged(nameof(EmployeeLoginData));
            }
        }
        private void EmployeeLogin(object? obj)
        {
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
