using System.Collections.ObjectModel;
using Supermarket.Wpf.Manager.AddProducts;
using Supermarket.Wpf.Manager.SupermarketCashboxes;
using Supermarket.Wpf.Manager.SupermarketEmployees;
using Supermarket.Wpf.Manager.SupermarketLogs;
using Supermarket.Wpf.Manager.SupermarketProducts;
using Supermarket.Wpf.Manager.SupermarketSales;
using Supermarket.Wpf.Manager.SupermarketStorages;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Manager
{
    public class ManagerMenuViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IViewModelResolver _viewModelResolver;
        public ObservableCollection<ITabViewModel> TabViewModels { get; } = new();
        
        private ITabViewModel? _selectedTabViewModel;
        public ITabViewModel? SelectedTabViewModel
        {
            get => _selectedTabViewModel;
            set
            {
                if (_selectedTabViewModel != value)
                {
                    SetProperty(ref _selectedTabViewModel, value);
                    _selectedTabViewModel?.ActivateIfNeeded();
                }
            }
        }
        
        public ManagerMenuViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            var supermarketProductsViewModel = await _viewModelResolver.Resolve<SupermarketProductsViewModel>();
            var addProductsViewModel = await _viewModelResolver.Resolve<AddProductsViewModel>();
            var supermarketEmployeesViewModel = await _viewModelResolver.Resolve<SupermarketEmployeesViewModel>();
            var supermarketStoragesViewModel = await _viewModelResolver.Resolve<SupermarketStoragesViewModel>();
            var supermarketSalesViewModel = await _viewModelResolver.Resolve<SupermarketSalesViewModel>();
            var supermarketCashboxesViewModel = await _viewModelResolver.Resolve<SupermarketCashboxesViewModel>();
            
            TabViewModels.Add(supermarketProductsViewModel);
            TabViewModels.Add(addProductsViewModel);
            TabViewModels.Add(supermarketEmployeesViewModel);
            TabViewModels.Add(supermarketStoragesViewModel);
            TabViewModels.Add(supermarketSalesViewModel);
            TabViewModels.Add(supermarketCashboxesViewModel);
            
            SelectedTabViewModel = supermarketProductsViewModel;
        }
    }
}
