using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Navigation
{
    internal class NavigationService : INavigationService
    {
        private readonly IViewModelResolver _viewModelResolver;
        private object? _currentViewModel;

        public NavigationService(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public ApplicationView? CurrentView { get; private set; }
        public event EventHandler<NavigationEventArgs>? NavigationSucceeded;

        public void NavigateTo(ApplicationView applicationView)
        {
            if (_currentViewModel is IConfirmNavigation confirmNavigation && confirmNavigation.CanNavigateFrom() == false)
            {
                confirmNavigation.NavigationCancelled();
            }
            else
            {
                var viewModelType = applicationView switch
                {
                    ApplicationView.Login => typeof(LoginViewModel),
                    ApplicationView.CashBox => typeof(CashboxViewModel),
                    _ => throw new NotImplementedException($"Navigation to {applicationView} is not supported yet, implement it by extending this swith")
                };
                
                _viewModelResolver.Resolve(viewModelType).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        throw task.Exception!;
                    }
                    
                    _currentViewModel = task.Result;
                    CurrentView = applicationView;
                    Debug.WriteLine($"Navigated to {CurrentView}");

                    NavigationSucceeded?.Invoke(this, new NavigationEventArgs
                    {
                        NewViewModel = _currentViewModel,
                    });
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
