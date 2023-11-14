﻿using Supermarket.Core.UseCases.ManagerMenu;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Manager.SupermarketProducts
{
    public class SupermarketProductsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ObservableCollection<ManagerMenuProduct> Products { get; } = new();
        
        public string TabHeader => "Naše produkty";

        public SupermarketProductsViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            var products = await _managerMenuService.GetSupermarketProducts(1, new RecordsRange { PageSize = 250, PageNumber = 1 });
            Products.Update(products.Items);
        }
    }
}
