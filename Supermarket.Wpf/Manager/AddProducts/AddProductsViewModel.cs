using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

            }
        }
    }
}
