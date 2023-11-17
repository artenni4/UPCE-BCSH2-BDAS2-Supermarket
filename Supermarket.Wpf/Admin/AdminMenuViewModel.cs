using Supermarket.Wpf.Admin.Employees;
using Supermarket.Wpf.Admin.ProductCategories;
using Supermarket.Wpf.Admin.Products;
using Supermarket.Wpf.Admin.Regions;
using Supermarket.Wpf.Admin.Supermarkets;
using Supermarket.Wpf.Admin.Suppliers;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Admin
{
    public class AdminMenuViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
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

        public AdminMenuViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            var suppliersViewModel = await _viewModelResolver.Resolve<AdminSuppliersViewModel>();
            var productsViewModel = await _viewModelResolver.Resolve<AdminProductsViewModel>();
            var supermarketsViewModel = await _viewModelResolver.Resolve<AdminSupermarketsViewModel>();
            var categoriesViewModel = await _viewModelResolver.Resolve<AdminMenuCategoriesViewModel>();
            var regionsViewModel = await _viewModelResolver.Resolve<AdminRegionsViewModel>();
            var employeesViewModel = await _viewModelResolver.Resolve<AdminEmployeesViewModel>();

            TabViewModels.Add(suppliersViewModel);
            TabViewModels.Add(productsViewModel);
            TabViewModels.Add(supermarketsViewModel);
            TabViewModels.Add(categoriesViewModel);
            TabViewModels.Add(regionsViewModel);
            TabViewModels.Add(employeesViewModel);

            SelectedTabViewModel = suppliersViewModel;
        }
    }
}
