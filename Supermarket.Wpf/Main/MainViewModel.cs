using System.Threading.Tasks;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Main
{
    public class MainViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ILoggedUserService _loggedUserService;
        
        public ICommand ToggleMenuOrCloseDialogCommand { get; }
        
        public MainViewModel(INavigationService navigationService,
            IDialogService dialogService,
            IViewModelResolver viewModelResolver,
            ILoggedUserService loggedUserService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _loggedUserService = loggedUserService;
            
            ToggleMenuOrCloseDialogCommand = new RelayCommand(ToggleMenuOrHideDialog);
            
            navigationService.NavigationSucceeded += NavigationSucceeded;
            
            dialogService.DialogShown += (_, args) => DialogViewModel = args.ViewModel;
            dialogService.DialogHidden += (_, _) => DialogViewModel = null;
            
            loggedUserService.EmployeeLoggedIn += async (_, _) => await TryShowMenu();

            viewModelResolver.ViewModelResolved += (_, args) =>
            {
                if (args.ViewModel is IAsyncViewModel asyncViewModel)
                {
                    asyncViewModel.LoadingStarted += (_, _) => IsProgressVisible = true;
                    asyncViewModel.LoadingFinished += (_, _) => IsProgressVisible = false;
                }
            };
        }

        public async Task InitializeAsync()
        {
            await _navigationService.NavigateToAsync(ApplicationView.Login);
        }
        
        private async void ToggleMenuOrHideDialog(object? obj)
        {
            if (_dialogService.CurrentDialog is not null)
            {
                _dialogService.Hide();
                return;
            }
            
            await TryShowMenu();
        }

        private async Task TryShowMenu()
        {
            if (_loggedUserService.LoggedEmployee is not null)
            {
                var applicationView = await _dialogService.ShowAsync<MenuViewModel, ApplicationView, ILoggedEmployee>(_loggedUserService.LoggedEmployee);
                await _navigationService.NavigateToAsync(applicationView);
            }
        }
        
        private void NavigationSucceeded(object? sender, NavigationEventArgs e)
        {
            _dialogService.Hide();
            ContentViewModel = e.NewViewModel;
        }
        
        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get => _isProgressVisible;
            private set => SetProperty(ref _isProgressVisible, value);
        }

        private IViewModel? _contentViewModel;
        public IViewModel? ContentViewModel
        {
            get => _contentViewModel;
            set => SetProperty(ref _contentViewModel, value);
        }

        private IViewModel? _dialogViewModel;
        public IViewModel? DialogViewModel
        {
            get => _dialogViewModel;
            set => SetProperty(ref _dialogViewModel, value);
        }
    }
}
