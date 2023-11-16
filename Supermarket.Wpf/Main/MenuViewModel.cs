using System.Windows.Input;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.Main
{
    public class MenuViewModel : NotifyPropertyChangedBase, IDialogViewModel<MenuResult>
    {
        public ICommand LogOutCommand { get; }
        public ICommand NavigateToCashboxCommand { get; }
        public ICommand NavigateToGoodsKeepingCommand { get; }
        public ICommand NavigateToManagerCommand { get; }
        public ICommand NavigateToAdminCommand { get; }

        public MenuViewModel(INavigationService navigationService, ILoggedUserService loggedUserService)
        {
            LoggedUserService = loggedUserService;
            
            LogOutCommand = new RelayCommand(_ => ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(MenuResult.LogOut())));
            
            NavigateToCashboxCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(MenuResult.Navigate(ApplicationView.CashBox))),
                _ => navigationService.CurrentView != ApplicationView.CashBox);

            NavigateToGoodsKeepingCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(MenuResult.Navigate(ApplicationView.Storage))),
                _ => navigationService.CurrentView != ApplicationView.Storage);

            NavigateToManagerCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(MenuResult.Navigate(ApplicationView.Manager))),
                _ => navigationService.CurrentView != ApplicationView.Manager);

            NavigateToAdminCommand = new RelayCommand(
                _ => ResultReceived?.Invoke(this, DialogResult<MenuResult>.Ok(MenuResult.Navigate(ApplicationView.Admin))),
                _ => navigationService.CurrentView != ApplicationView.Admin);
        }

        private ILoggedUserService? _loggedUserService;
        public ILoggedUserService? LoggedUserService
        {
            get => _loggedUserService;
            private set => SetProperty(ref _loggedUserService, value);
        }

        public event EventHandler<DialogResult<MenuResult>>? ResultReceived;
    }
}
