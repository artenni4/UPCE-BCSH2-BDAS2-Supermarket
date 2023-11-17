using Supermarket.Core.UseCases.ManagerMenu;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Wpf.LoggedUser;

namespace Supermarket.Wpf.Manager.AddProducts
{
    public class AddProductsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public ObservableCollection<ManagerMenuAddProduct> Products { get; set; }

        public ICommand CheckBoxClickCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Přidat zboží";

        public AddProductsViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Products = new();

            CheckBoxClickCommand = new RelayCommand(HandleCheckBoxClick);
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            var products = await _managerMenuService.GetManagerProductsToAdd(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });
            Products.Update(products.Items);
        }

        private async void HandleCheckBoxClick(object? obj)
        {
            if (obj is ManagerMenuAddProduct product)
            {
                using var _ = new DelegateLoading(this);
                
                if (product.IsInSupermarket) // remove from supermarket
                {
                    await _managerMenuService.RemoveProductFromSupermarket(new StoredProductId { ProductId = product.ProductId, StoragePlaceId = product.StoragePlaceId, SupermarketId = _loggedUserService.SupermarketId});
                }
                else // add to supermarket
                {
                    await _managerMenuService.AddProductToSupermarket(new SellingProductId { ProductId = product.ProductId, SupermarketId = _loggedUserService.SupermarketId });
                }
            }
        }
    }
}
