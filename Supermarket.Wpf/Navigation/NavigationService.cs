using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MainViewModel _mainViewModel;

        public NavigationService(IServiceProvider serviceProvider, MainViewModel mainViewModel)
        {
            _serviceProvider = serviceProvider;
            _mainViewModel = mainViewModel;
        }

        public void NavigateTo(NavigateWindow navigateWindow)
        {
            _mainViewModel.CurrentViewModel = navigateWindow switch
            {
                NavigateWindow.Login => _serviceProvider.GetRequiredService<LoginViewModel>(),
                NavigateWindow.CashBox => _serviceProvider.GetRequiredService<CashboxViewModel>(),
                _ => throw new NotImplementedException($"Navigation to {navigateWindow} is not supported yet, implement it by extending this swith")
            };
        }
    }
}
