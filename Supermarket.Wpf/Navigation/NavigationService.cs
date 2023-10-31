using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using System;

namespace Supermarket.Wpf.Navigation
{
    internal class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object? currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public NavigateWindow? CurrentWindow { get; private set; }
        public event EventHandler<NavigationEventArgs>? NavigationCompleted;

        public void NavigateTo(NavigateWindow navigateWindow)
        {
            if (currentViewModel is IConfirmNavigation confirmNavigation && confirmNavigation.CanNavigateFrom() == false)
            {
                confirmNavigation.NavigationCancelled();
            }
            else
            {
                currentViewModel = navigateWindow switch
                {
                    NavigateWindow.Login => _serviceProvider.GetRequiredService<LoginViewModel>(),
                    NavigateWindow.CashBox => _serviceProvider.GetRequiredService<CashboxViewModel>(),
                    _ => throw new NotImplementedException($"Navigation to {navigateWindow} is not supported yet, implement it by extending this swith")
                };
                CurrentWindow = navigateWindow;

                NavigationCompleted?.Invoke(this, new NavigationEventArgs
                {
                    NewViewModel = currentViewModel,
                });
            }

        }
    }
}
