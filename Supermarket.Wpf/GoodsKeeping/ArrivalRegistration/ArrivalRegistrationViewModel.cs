using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.Wpf.GoodsKeeping.ArrivalRegistration
{
    public class ArrivalRegistrationViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IGoodsKeepingService _goodsKeepingService;
        private readonly IDialogService _dialogService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        private int currentPage = 1;
        private int? categoryId;

        private PagedResult<GoodsKeepingProduct>? products;
        private PagedResult<GoodsKeepingProductCategory>? categories;
        private PagedResult<SupplyWarehouse>? storagePlaces;

        public ObservableCollection<GoodsKeepingProduct> DisplayedProducts { get; set; }
        public ObservableCollection<GoodsKeepingProductCategory> Categories { get; set; }
        public ObservableCollection<GoodsKeepingProduct> SelectedProducts { get; set; }
        public ObservableCollection<SupplyWarehouse> StoragePlaces { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand CategoryButtonClickCommand { get; }
        public ICommand ProductClickCommand { get; }
        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RemoveProductCommand { get; }

        private SupplyWarehouse? _selectedPlace;
        public SupplyWarehouse? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                OnPropertyChanged();
            }
        }

        public string TabHeader => "Příjezdy";

        public ArrivalRegistrationViewModel(IGoodsKeepingService goodsKeepingService, IDialogService dialogService)
        {
            _goodsKeepingService = goodsKeepingService;
            _dialogService = dialogService;

            DisplayedProducts = new();
            Categories = new();
            SelectedProducts = new();
            StoragePlaces = new();

            NextPageCommand = new RelayCommand(NextPage, _ => products?.HasNext == true);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => products?.HasPrevious == true);
            CategoryButtonClickCommand = new RelayCommand(CategoryButtonClick);
            ProductClickCommand = new RelayCommand(ProductClick);
            AcceptCommand = new RelayCommand(AcceptClick);
            CancelCommand = new RelayCommand(CancelClick);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            categories = await _goodsKeepingService.GetCategoriesAsync(1, new RecordsRange { PageSize = 10, PageNumber = 1 });
            categoryId = categories.Items.FirstOrDefault()?.CategoryId;
            storagePlaces = await _goodsKeepingService.GetWarehousesAsync(1, new RecordsRange { PageSize = 30, PageNumber = 1 });
            for (int i = 0; i < categories.Items.Count; i++)
            {
                Categories.Add(categories.Items[i]);
            }

            for (int i = 0; i < storagePlaces.Items.Count; i++)
            {
                StoragePlaces.Add(storagePlaces.Items[i]);
            }

            await UpdateDisplayedItems();
        }

        public async void NextPage(object? obj)
        {
            using var _ = new DelegateLoading(this);

            currentPage++;
            await UpdateDisplayedItems();
        }

        public async void PreviousPage(object? obj)
        {
            using var _ = new DelegateLoading(this);

            currentPage--;
            await UpdateDisplayedItems();
        }

        private async Task UpdateDisplayedItems()
        {
            if (categoryId.HasValue == false)
            {
                return;
            }

            products = await _goodsKeepingService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = currentPage }, categoryId.Value, null);
            DisplayedProducts.Clear();

            for (int i = 0; i < products.Items.Count; i++)
            {
                DisplayedProducts.Add(products.Items[i]);
            }
        }

        private async void CategoryButtonClick(object? obj)
        {
            using var _ = new DelegateLoading(this);

            if (obj is GoodsKeepingProductCategory selectedCategory)
            {
                categoryId = selectedCategory.CategoryId;
                await UpdateDisplayedItems();
            }
        }

        public async void ProductClick(object? obj)
        {
            if (obj is GoodsKeepingProduct selectedProduct)
            {
                var dialogResult = await _dialogService.ShowInputDialogAsync<decimal>(title: "POČET", inputLabel: selectedProduct.MeasureUnit);

                if (dialogResult.IsOk(out var productCount))
                {
                    SelectedProducts.Add(new GoodsKeepingProduct
                    {
                        ProductId = selectedProduct.ProductId,
                        Name = selectedProduct.Name,
                        Weight = productCount,
                        MeasureUnit = selectedProduct.MeasureUnit,
                        IsByWeight = selectedProduct.IsByWeight,
                        Price = selectedProduct.Price
                    });
                }
            }
        }

        public async void AcceptClick(object? obj)
        {
            if (SelectedPlace == null)
            {
                MessageBox.Show("Vyberte místo uložení", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<SuppliedProduct> products = new List<SuppliedProduct>();
            foreach(var product in SelectedProducts)
            {
                products.Add(new SuppliedProduct { ProductId = product.ProductId, Count = product.Weight });
            }

            if (SelectedPlace != null)
                await _goodsKeepingService.SupplyProductsToWarehouseAsync(SelectedPlace.Id, products);
            SelectedProducts.Clear();
        }

        public void CancelClick(object? obj)
        {
            SelectedProducts.Clear();
        }

        private void RemoveProduct(object? parameter)
        {
            if (parameter is GoodsKeepingProduct item)
            {
                SelectedProducts.Remove(item);
            }
        }
    }
}
