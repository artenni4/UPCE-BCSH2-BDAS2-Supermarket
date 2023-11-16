using Supermarket.Wpf.CashBox;
using Supermarket.Wpf.Login;
using System.Diagnostics;
using Supermarket.Wpf.ViewModelResolvers;
using Supermarket.Wpf.GoodsKeeping;
using Supermarket.Wpf.Manager;
using Supermarket.Wpf.Admin;

namespace Supermarket.Wpf.Navigation
{
    internal class NavigationService : INavigationService
    {
        private readonly IViewModelResolver _viewModelResolver;
        private IViewModel? _currentViewModel;

        public NavigationService(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        private ApplicationView? _previousView;
        public ApplicationView? CurrentView { get; private set; }
        public event EventHandler<NavigationEventArgs>? NavigationSucceeded;

        public async Task NavigateToAsync(ApplicationView applicationView)
        {
            await NavigateInternal(applicationView, back: false);
        }

        public async Task BackAsync()
        {
            if (_previousView.HasValue)
            {
                await NavigateInternal(_previousView.Value, back: true);
            }
        }

        private async Task NavigateInternal(ApplicationView applicationView, bool back)
        {
            if (ConfirmNavigation() == false)
            {
                return;
            }

            _currentViewModel = await ResolveViewModel(applicationView);

            _previousView = back ? null : CurrentView;
            CurrentView = applicationView;
            Debug.WriteLine($"Navigated to {CurrentView}");
            NavigationSucceeded?.Invoke(this, new NavigationEventArgs
            {
                NewViewModel = _currentViewModel,
            });
        }
        
        private async Task<IViewModel> ResolveViewModel(ApplicationView applicationView)
        {
            var viewModelType = applicationView switch
            {
                ApplicationView.Login => typeof(LoginViewModel),
                ApplicationView.CashBox => typeof(CashBoxViewModel),
                ApplicationView.Storage => typeof(StorageViewModel),
                ApplicationView.Manager => typeof(ManagerMenuViewModel),
                ApplicationView.Admin => typeof(AdminMenuViewModel),
                _ => throw new NotSupportedException($"Navigation to {applicationView} is not supported yet, implement it by extending this swith")
            };
            
            return await _viewModelResolver.Resolve(viewModelType);
        }

        private bool ConfirmNavigation()
        {
            if (_currentViewModel is IConfirmNavigation confirmNavigation && confirmNavigation.CanNavigateFrom() == false)
            {
                confirmNavigation.NavigationCancelled();
                return false;
            }

            return true;
        }
    }
}
