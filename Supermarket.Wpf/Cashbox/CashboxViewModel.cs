using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Wpf.Cashbox.ProductInput;
using Supermarket.Wpf.Common;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel : NotifyPropertyChangedBase
    {
        private readonly ICashBoxService _cashBoxService;
        private int currentPage = 1;
        private int categoryId = 1;

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
            this._cashBoxService = cashBoxService;
            DisplayedProducts = new();
            Categories = new();
            SelectedProducts = new();
            UpdateDisplayedItems();

            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            CategoryButtonClickCommand = new RelayCommand(CategoryButtonClick);
            ProductClickCommand = new RelayCommand(ProductClick);

            categories = _cashBoxService.GetCategoriesAsync(1, new RecordsRange { PageSize = 10, PageNumber = 1 }).Result;
            for (int i = 0; i < categories.Items.Count; i++)
            {
                Categories.Add(categories.Items[i]);
            }
        }

        public void NextPage(object? obj)
        {
            if (products?.HasNext == true)
            {
                currentPage++;
                UpdateDisplayedItems();
            }
        }

        public void PreviousPage(object? obj)
        {
            if (products?.HasPrevious == true)
            {
                currentPage--;
                UpdateDisplayedItems();
            }
        }

        // TODO: Async
        private void UpdateDisplayedItems()
        {
            products = _cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = currentPage }, categoryId, null).Result;
            DisplayedProducts.Clear();

            for (int i = 0; i < products.Items.Count; i++)
            {
                DisplayedProducts.Add(products.Items[i]);

            }
        }

        public void CategoryButtonClick(object? obj)
        {
            if (obj is CashBoxProductCategory selectedCategory)
            {
                categoryId = selectedCategory.Id;
                UpdateDisplayedItems();
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
