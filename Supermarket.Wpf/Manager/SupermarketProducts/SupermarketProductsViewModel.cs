﻿using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.LoggedUser;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Manager.SupermarketProducts
{
    public class SupermarketProductsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        private BestSellingProduct? _bestSellingProduct;

        public BestSellingProduct? BestSellingProduct
        {
            get => _bestSellingProduct;
            private set => SetProperty(ref _bestSellingProduct, value);
        }
        public ObservableCollection<ManagerMenuProduct> Products { get; } = new();
        
        public string TabHeader => "Naše produkty";

        public SupermarketProductsViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            var products = await _managerMenuService.GetSupermarketProducts(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });
            Products.Update(products.Items);
            BestSellingProduct = await _managerMenuService.GetBestSellingProduct(_loggedUserService.SupermarketId);
        }
    }
}
