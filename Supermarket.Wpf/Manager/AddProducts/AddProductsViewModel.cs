using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.SellingProducts;

namespace Supermarket.Wpf.Manager.AddProducts
{
    public class AddProductsViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;

        private PagedResult<ManagerMenuAddProduct>? products;
        public ObservableCollection<ManagerMenuAddProduct> Products { get; set; }

        public ICommand CheckBoxClickCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AddProductsViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;

            Products = new();

            CheckBoxClickCommand = new RelayCommand(HandleCheckBoxClick);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            products = await _managerMenuService.GetManagerProductsToAdd(1, new RecordsRange { PageSize = 250, PageNumber = 1 });
            foreach (var product in products.Items)
            {
                Products.Add(product);
            }
        }

        private void HandleCheckBoxClick(object? obj)
        {
            if (obj is ManagerMenuAddProduct product)
            {
                if (product.IsInSupermarket) // remove from supermarket
                {
                    _managerMenuService.RemoveProductFromSupermarket(new StoredProductId { ProductId = product.ProductId, StoragePlaceId = product.StoragePlaceId, SupermarketId = 1});
                }
                else // add to supermarket
                {
                    _managerMenuService.AddProductToSupermarket(new SellingProductId { ProductId = product.ProductId, SupermarketId = 1 });
                }
            }
        }


    }
}
