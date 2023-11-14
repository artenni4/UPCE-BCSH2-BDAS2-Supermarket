using Supermarket.Core.UseCases.ManagerMenu;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.SellingProducts;

namespace Supermarket.Wpf.Manager.AddProducts
{
    public class AddProductsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;

        public ObservableCollection<ManagerMenuAddProduct> Products { get; set; }

        public ICommand CheckBoxClickCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Přidat zboží";

        public AddProductsViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;

            Products = new();

            CheckBoxClickCommand = new RelayCommand(HandleCheckBoxClick);
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            var products = await _managerMenuService.GetManagerProductsToAdd(1, new RecordsRange { PageSize = 250, PageNumber = 1 });
            Products.Update(products.Items);
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
