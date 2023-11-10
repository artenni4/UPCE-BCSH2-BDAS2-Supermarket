using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Manager.SupermarketProducts
{
    public class SupermarketProductsViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        private PagedResult<ManagerMenuProduct>? products;
        public ObservableCollection<ManagerMenuProduct> Products { get; set; }

        public SupermarketProductsViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;

            Products = new();

        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            products = await _managerMenuService.GetSupermarketProducts(1, new RecordsRange { PageSize = 250, PageNumber = 1 });
            foreach(var product in products.Items)
            {
                Products.Add(product);
            }
        }
    }
}
