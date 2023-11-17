using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Core.UseCases.Login;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.Menu;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Login
{
    internal class LoginViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly ILoginService _loginService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;
        private readonly IMenuService _menuService;
        private readonly INavigationService _navigationService;

        private PagedResult<CustomerSupermarket> _supermarkets = PagedResult<CustomerSupermarket>.Empty();
        
        public ICommand EmployeeLoginCommand { get; }
        public ICommand CustomerLoginCommand { get; }

        public LoginViewModel(ILoginService loginService, ILoggedUserService loggedUserService, IDialogService dialogService, INavigationService navigationService, IMenuService menuService)
        {
            _loginService = loginService;
            _loggedUserService = loggedUserService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _menuService = menuService;

            EmployeeLoginCommand = new RelayCommand(EmployeeLoginAsync, CanEmployeeLogin);
            CustomerLoginCommand = new RelayCommand(CustomerLogin);
        }
        
        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            
            _supermarkets = await _loginService.GetSupermarketsAsync(
                new RecordsRange { PageNumber = 1, PageSize = 100 });
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
                    _loggedUserService.SetAdmin(loggedAdmin);
                }
            }
            catch (InvalidCredentialsException)
            {
                MessageBox.Show("Špatné příhlašovací údaje", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            await _menuService.TryShowMenuAsync();
        }

        private async void CustomerLogin(object? obj)
        {
            var dialogResult = await _dialogService.ShowDropDownDialogAsync("Zvolte supermarket",
                nameof(CustomerSupermarket.Address), _supermarkets.Items);

            if (!dialogResult.IsOk(out var supermarket))
            {
                return;
            }
            
            _loggedUserService.SetCustomer(supermarket.Id);
            await _navigationService.NavigateToAsync(ApplicationView.CashBox);
        }

        private bool CanEmployeeLogin(object? arg) => !string.IsNullOrWhiteSpace(EmployeeLoginData.Login) && !string.IsNullOrWhiteSpace(EmployeeLoginData.Password);
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
    }
}
