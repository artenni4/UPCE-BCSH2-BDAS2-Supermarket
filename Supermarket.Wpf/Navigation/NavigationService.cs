using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(MainViewModel mainViewModel, IServiceProvider serviceProvider)
        {
            _mainViewModel = mainViewModel;
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo(string viewModelName)
        {
            // Instantiate the target ViewModel via Reflection and Dependency Injection
            if (viewModelName == "CashboxViewModel")
            {
                object? viewModel = _serviceProvider.GetRequiredService<CashboxViewModel>();
                _mainViewModel.CurrentViewModel = viewModel;

            }

            // Update the current ViewModel
        }
    }
}
