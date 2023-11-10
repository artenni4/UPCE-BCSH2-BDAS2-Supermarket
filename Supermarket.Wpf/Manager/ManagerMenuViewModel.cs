using Supermarket.Wpf.Common;
using Supermarket.Wpf.Manager.AddProducts;
using Supermarket.Wpf.Manager.SupermarketProducts;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Manager
{
    public class ManagerMenuViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IViewModelResolver _viewModelResolver;

        private SupermarketProductsViewModel? _supermarketProductsViewModel;
        public SupermarketProductsViewModel? SupermarketProductsViewModel
        {
            get => _supermarketProductsViewModel;
            set => SetProperty(ref _supermarketProductsViewModel, value);
        }

        private AddProductsViewModel? _addProductsViewModel;
        public AddProductsViewModel? AddProductsViewModel
        {
            get => _addProductsViewModel;
            set => SetProperty(ref _addProductsViewModel, value);
        }

        public ManagerMenuViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            SupermarketProductsViewModel = await _viewModelResolver.Resolve<SupermarketProductsViewModel>();
            AddProductsViewModel = await _viewModelResolver.Resolve<AddProductsViewModel>();
        }
    }
}
