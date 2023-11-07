using System;
using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Wpf.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly ICashBoxService _cashBoxService;
        private readonly IDialogService _dialogService;
        
        private int currentPage = 1;
        private int? categoryId;

        private PagedResult<CashBoxProduct>? products;
        private PagedResult<CashBoxProductCategory>? categories;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ObservableCollection<CashBoxProduct> DisplayedProducts { get; set; }
        public ObservableCollection<CashBoxProductCategory> Categories { get; set; }
        public ObservableCollection<CashBoxProduct> SelectedProducts { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand CategoryButtonClickCommand { get; }
        public ICommand ProductClickCommand { get; }

        public CashboxViewModel(ICashBoxService cashBoxService, IDialogService dialogService)
        {
            _cashBoxService = cashBoxService;
            _dialogService = dialogService;
            DisplayedProducts = new();
            Categories = new();
            SelectedProducts = new();

            NextPageCommand = new RelayCommand(NextPage, _ => products?.HasNext == true);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => products?.HasPrevious == true);
            CategoryButtonClickCommand = new RelayCommand(CategoryButtonClick);
            ProductClickCommand = new RelayCommand(ProductClick);
        }
        
        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            
            categories = await _cashBoxService.GetCategoriesAsync(1, new RecordsRange { PageSize = 10, PageNumber = 1 });
            categoryId = categories.Items.FirstOrDefault()?.CategoryId;
            for (int i = 0; i < categories.Items.Count; i++)
            {
                Categories.Add(categories.Items[i]);
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
            
            products = await _cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = currentPage }, categoryId.Value, null);
            DisplayedProducts.Clear();

            for (int i = 0; i < products.Items.Count; i++)
            {
                DisplayedProducts.Add(products.Items[i]);
            }
        }

        private async void CategoryButtonClick(object? obj)
        {
            using var _ = new DelegateLoading(this);
            
            if (obj is CashBoxProductCategory selectedCategory)
            {
                currentPage = 1;
                categoryId = selectedCategory.CategoryId;
                await UpdateDisplayedItems();
            }
        }

        public async void ProductClick(object? obj)
        {
            if (obj is CashBoxProduct selectedProduct)
            {
                var result = await _dialogService
                    .ShowForResultAsync<ProductCountInputViewModel, DialogResult<decimal>, EmptyParameters>(EmptyParameters.Value);
                
                SelectedProducts.Add(selectedProduct);
            }
        }
    }
}
