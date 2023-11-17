using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Core.UseCases.ApplicationMenu;
using Supermarket.Core.UseCases.Login;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Menu
{
    public class MenuViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized, IDialogViewModel<MenuResult>
    {
        private readonly IApplicationMenuService _applicationMenuService;
        private readonly IDialogService _dialogService;
        private readonly ILoggedUserService _loggedUserService;
        
        public ICommand LogOutCommand { get; }
        public ICommand NavigateToCashboxCommand { get; }
        public ICommand NavigateToGoodsKeepingCommand { get; }
        public ICommand NavigateToManagerCommand { get; }
        public ICommand NavigateToAdminCommand { get; }
        
        public ILoggedUserService LoggedUserService { get; }
        
        private IReadOnlyList<AdminSupermarket>? _adminSupermarkets;
        
        public MenuViewModel(INavigationService navigationService,
            ILoggedUserService loggedUserService,
            IApplicationMenuService applicationMenuService, 
            IDialogService dialogService)
        {
            LoggedUserService = loggedUserService;
            _loggedUserService = loggedUserService;
            _applicationMenuService = applicationMenuService;
            _dialogService = dialogService;

            LogOutCommand = new RelayCommand(LogOut);
            
            NavigateToCashboxCommand = new RelayCommand(NavigateToCashbox,
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            NavigateToGoodsKeepingCommand = new RelayCommand(NavigateToGoodsKeeping,
                _ => navigationService.CurrentView != ApplicationView.Storage);

            NavigateToManagerCommand = new RelayCommand(NavigateToManager,
                _ => navigationService.CurrentView != ApplicationView.Manager);

            NavigateToAdminCommand = new RelayCommand(NavigateToAdmin,
                _ => navigationService.CurrentView != ApplicationView.Admin);
        }

        private void LogOut(object? obj)
        {
            var menuResult = MenuResult.LogOut();
            ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(menuResult));
        }
        
        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            
            var supermarkets = await _applicationMenuService
                .GetSupermarketsForAdminAsync(new RecordsRange { PageNumber = 1, PageSize = 250 });
            
            _adminSupermarkets = supermarkets.Items;
        }

        private async Task NavigateTo(ApplicationView applicationView)
        {
            if (applicationView != ApplicationView.Admin && LoggedUserService.IsAdmin(out _))
            {
                if (_adminSupermarkets is null)
                {
                    return;
                }
                
                var dialogResult = await _dialogService.ShowDropDownDialogAsync("Zvolte supermarket",
                    nameof(CustomerSupermarket.Address), _adminSupermarkets);

                if (dialogResult.IsOk(out var supermarket))
                {
                    _loggedUserService.SetAdminSupermarket(supermarket.Id);
                }
                else
                {
                    return;
                }
            }

            var menuResult = MenuResult.Navigate(applicationView);
            ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(menuResult));
        }

        private async void NavigateToCashbox(object? obj)
        {
            await NavigateTo(ApplicationView.CashBox);
        }
        
        private async void NavigateToGoodsKeeping(object? obj)
        {
            await NavigateTo(ApplicationView.Storage);
        }
        
        private async void NavigateToManager(object? obj)
        {
            await NavigateTo(ApplicationView.Manager);
        }
        
        private async void NavigateToAdmin(object? obj)
        {
            await NavigateTo(ApplicationView.Admin);
        }

        public event EventHandler<DialogResult<MenuResult>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
    }
}
