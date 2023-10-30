using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Wpf.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel : NotifyPropertyChangedBase
    {
        private readonly ICashBoxService _cashBoxService;
        private int currentPage = 1;

        private PagedResult<CashBoxProduct>? products;


        public ObservableCollection<CashBoxProduct> DisplayedProducts { get; set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public CashboxViewModel(ICashBoxService cashBoxService)
        {
            this._cashBoxService = cashBoxService;
            DisplayedProducts = new();
            UpdateDisplayedItems();

            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
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

        private void UpdateDisplayedItems()
        {
            products = _cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = currentPage }, 1, null).Result;
            DisplayedProducts.Clear();

            for (int i = 0; i < products.Items.Count; i++)
            {
                DisplayedProducts.Add(products.Items[i]);

            }
        }

    }
}
