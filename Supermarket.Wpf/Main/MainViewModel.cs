using System;
using System.Diagnostics;
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
    public class MainViewModel : NotifyPropertyChangedBase, IViewModel
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
            
            viewModelResolver.InitializationStarted += (_, _) => IsProgressVisible = true;
            viewModelResolver.InitializationFinished += (_, _) => IsProgressVisible = false;
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
                _navigationService.NavigateTo(applicationView);
            }
        }
        
        private void NavigationSucceeded(object? sender, NavigationEventArgs e)
        {
            _dialogService.Hide();
            ContentViewModel = e.NewViewModel;
            Debug.WriteLine($"{nameof(ContentViewModel)} in {nameof(MainViewModel)} is set new value due to navigation completion");
        }
        
        private bool isProgressVisible;
        public bool IsProgressVisible
        {
            get => isProgressVisible;
            private set => SetProperty(ref isProgressVisible, value);
        }

        private IViewModel? _contentViewModel;
        public IViewModel? ContentViewModel
        {
            get => _contentViewModel;
            set => SetProperty(ref _contentViewModel, value);
        }

        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get => _isDialogVisible;
            set => SetProperty(ref _isDialogVisible, value);
        }

        private IViewModel? _dialogViewModel;
        public IViewModel? DialogViewModel
        {
            get => _dialogViewModel;
            set => SetProperty(ref _dialogViewModel, value);
        }
    }
}
