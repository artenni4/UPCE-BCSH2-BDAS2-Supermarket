using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Wpf.Cashbox.ProductInput;
using Supermarket.Wpf.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly ICashBoxService _cashBoxService;
        private int currentPage = 1;
        private int? categoryId;

        private PagedResult<CashBoxProduct>? products;
        private PagedResult<CashBoxProductCategory>? categories;


        public ObservableCollection<CashBoxProduct> DisplayedProducts { get; set; }
        public ObservableCollection<CashBoxProductCategory> Categories { get; set; }
        public ObservableCollection<CashBoxProduct> SelectedProducts { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand CategoryButtonClickCommand { get; }
        public ICommand ProductClickCommand { get; }

        public CashboxViewModel(ICashBoxService cashBoxService)
        {
            _cashBoxService = cashBoxService;
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
            if (products?.HasNext == true)
            {
                currentPage++;
                await UpdateDisplayedItems();
            }
        }

        public async void PreviousPage(object? obj)
        {
            if (products?.HasPrevious == true)
            {
                currentPage--;
                await UpdateDisplayedItems();
            }
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

        public async void CategoryButtonClick(object? obj)
        {
            if (obj is CashBoxProductCategory selectedCategory)
            {
                categoryId = selectedCategory.CategoryId;
                await UpdateDisplayedItems();
            }
        }

        public void ProductClick(object? obj)
        {
            if (obj is CashBoxProduct selectedProduct)
            {
                SelectedProducts.Add(selectedProduct);

                

                ProductInputWindow productInputWindow = new ProductInputWindow();
                
            }
        }
    }
}
