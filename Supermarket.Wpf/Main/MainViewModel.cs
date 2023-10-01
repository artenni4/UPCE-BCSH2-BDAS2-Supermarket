using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Main
{
    public class MainViewModel : INotifyPropertyChanged, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _currentViewModel = serviceProvider.GetRequiredService<CashboxViewModel>();
            _serviceProvider = serviceProvider;
        }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        public void ShowCustomerCashbox()
        {
            CurrentViewModel = new CashboxViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NavigateTo(string viewModelName)
        {
            if (viewModelName == "CashboxViewModel")
            {
                object? viewModel = _serviceProvider.GetRequiredService<CashboxViewModel>();
                _currentViewModel = viewModel;

            }
        }
    }
}
