using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Core.Domain.Products;
using Supermarket.Wpf.LoggedUser;

namespace Supermarket.Wpf.GoodsKeeping.GoodsManagement
{
    public class GoodsManagementViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly IGoodsKeepingService _goodsKeepingService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<GoodsKeepingStoredProduct> StoredProducts { get; set; }

        private GoodsKeepingStoredProduct? _selectedStoredProduct;
        public GoodsKeepingStoredProduct? SelectedStoredProduct
        {
            get { return _selectedStoredProduct; }
            set
            {
                if (_selectedStoredProduct != value)
                {
                    _selectedStoredProduct = value;
                    OnPropertyChanged(nameof(SelectedStoredProduct));
                }
            }
        }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ICommand MoveCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Goods management";

        public GoodsManagementViewModel(IGoodsKeepingService goodsKeepingService, IDialogService dialogService, ILoggedUserService loggedUserService)
        {
            _goodsKeepingService = goodsKeepingService;
            _dialogService = dialogService;
            _loggedUserService = loggedUserService;

            StoredProducts = new();

            MoveCommand = new RelayCommand(MoveProduct, CanCallDialog);
            DeleteCommand = new RelayCommand(DeleteProduct, CanCallDialog);
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            var storedProducts = await _goodsKeepingService
                .GetStoredProducts(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 600, PageNumber = 1 });
            StoredProducts.Update(storedProducts.Items);
        }

        public async void MoveProduct(object? obj)
        {
            if (SelectedStoredProduct == null)
            {
                return;
            }
            
            var result = await _dialogService.ShowAsync<MoveStoredProductViewModel, MoveProduct, MeasureUnit>(SelectedStoredProduct.MeasureUnit);
            if (result.IsOk(out var moveResult))
            {
                await _goodsKeepingService.MoveProductAsync(SelectedStoredProduct.StoragePlaceId, new MovingProduct
                {
                    Count = moveResult.Count,
                    NewStoragePlaceId = moveResult.StorageId,
                    ProductId = SelectedStoredProduct.ProductId,
                });
                await ActivateAsync();
            }
        }

        public async void DeleteProduct(object? obj)
        {
            if (SelectedStoredProduct == null)
            {
                return;
            }
            
            var dialogResult = await _dialogService.ShowProductCountDialog(SelectedStoredProduct.MeasureUnit);
            if (!dialogResult.IsOk(out var count))
            {
                return;
            }
            await _goodsKeepingService.DeleteProductStorageAsync(SelectedStoredProduct.StoragePlaceId, SelectedStoredProduct.ProductId, count);
            await ActivateAsync();
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedStoredProduct != null;
        }

        
    }
}
