using System.Collections.ObjectModel;
using Supermarket.Wpf.Navigation;
using System.Windows.Input;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Main
{
    public class MainViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ILoggedUserService _loggedUserService;
        
        public ICommand ToggleMenuOrCloseDialogCommand { get; }

        public ObservableCollection<IViewModel> DialogStack { get; } = new();
        
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
            
            dialogService.DialogShown += (_, args) => DialogStack.Add(args.ViewModel);
            dialogService.DialogHidden += (_, _) => DialogStack.RemoveAt(DialogStack.Count - 1);

            loggedUserService.UserLoggedIn += UserLoggedIn;

            viewModelResolver.ViewModelResolved += ViewModelResolved;
        }

        private async void UserLoggedIn(object? sender, EventArgs e)
        {
            if (ContentViewModel is not LoginViewModel)
            {
                return;
            }

            if (_loggedUserService.IsCustomer)
            {
                await _navigationService.NavigateToAsync(ApplicationView.CashBox);
                return;
            }
                
            await TryShowMenu();
        }

        private void ViewModelResolved(object? sender, ResolvedViewModelEventArgs e)
        {
            if (e.ViewModel is not IAsyncViewModel asyncViewModel)
            {
                return;
            }
            
            asyncViewModel.LoadingStarted += (_, _) => IsProgressVisible = true;
            asyncViewModel.LoadingFinished += (_, _) => IsProgressVisible = false;
        }

        public async Task InitializeAsync()
        {
            await _navigationService.NavigateToAsync(ApplicationView.Login);
        }
        
        private async void ToggleMenuOrHideDialog(object? obj)
        {
            if (_dialogService.DisplayedDialogs.Any())
            {
                _dialogService.Hide();
                return;
            }
            
            await TryShowMenu();
        }

        private async Task TryShowMenu()
        {
            if (!_loggedUserService.IsUserSet || _loggedUserService.IsCustomer)
            {
                return;
            }
            
            var result = await _dialogService.ShowAsync<MenuViewModel, MenuResult>();
            if (result.IsOk(out var menuResult))
            {
                if (menuResult.IsNavigate(out var applicationView))
                {
                    await _navigationService.NavigateToAsync(applicationView);
                }
                else if (menuResult.IsLogOut())
                {
                    await _navigationService.NavigateToAsync(ApplicationView.Login);
                    _loggedUserService.UnsetUser();
                }
            }
        }
        
        private async void NavigationSucceeded(object? sender, NavigationEventArgs e)
        {
            ContentViewModel = e.NewViewModel;
            await ContentViewModel.ActivateIfNeeded();
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
            private set => SetProperty(ref _contentViewModel, value);
        }
    }
}
