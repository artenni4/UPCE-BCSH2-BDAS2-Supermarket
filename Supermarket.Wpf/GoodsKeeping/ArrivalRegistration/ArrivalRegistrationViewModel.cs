using Supermarket.Core.CashBoxes;
using Supermarket.Core.GoodsKeeping;
using Supermarket.Domain.Common.Paging;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.GoodsKeeping.ArrivalRegistration
{
    public class ArrivalRegistrationViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        private readonly IGoodsKeepingService _goodsKeepingService;
        private int currentPage = 1;
        private int? categoryId;

        private PagedResult<GoodsKeepingProduct>? products;
        private PagedResult<GoodsKeepingProductCategory>? categories;
        private PagedResult<GoodsKeepingStoragePlace>? storagePlaces;

        public ObservableCollection<GoodsKeepingProduct> DisplayedProducts { get; set; }
        public ObservableCollection<GoodsKeepingProductCategory> Categories { get; set; }
        public ObservableCollection<GoodsKeepingProduct> SelectedProducts { get; set; }
        public ObservableCollection<GoodsKeepingStoragePlace> StoragePlaces { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand CategoryButtonClickCommand { get; }
        public ICommand ProductClickCommand { get; }

        private GoodsKeepingStoragePlace? _selectedPlace;
        public GoodsKeepingStoragePlace? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                OnPropertyChanged();
            }
        }

        public ArrivalRegistrationViewModel(IGoodsKeepingService goodsKeepingService)
        {
            _goodsKeepingService = goodsKeepingService;
            DisplayedProducts = new();
            Categories = new();
            SelectedProducts = new();
            StoragePlaces = new();

            NextPageCommand = new RelayCommand(NextPage, _ => products?.HasNext == true);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => products?.HasPrevious == true);
            CategoryButtonClickCommand = new RelayCommand(CategoryButtonClick);
            ProductClickCommand = new RelayCommand(ProductClick);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            categories = await _goodsKeepingService.GetCategoriesAsync(1, new RecordsRange { PageSize = 10, PageNumber = 1 });
            categoryId = categories.Items.FirstOrDefault()?.Id;
            storagePlaces = await _goodsKeepingService.GetStoragePlacesAsync(1, new RecordsRange { PageSize = 30, PageNumber = 1 });
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
                categoryId = selectedCategory.Id;
                await UpdateDisplayedItems();
            }
        }

        public void ProductClick(object? obj)
        {
            if (obj is GoodsKeepingProduct selectedProduct)
            {
                SelectedProducts.Add(selectedProduct);


            }
        }
    }
}
