using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel
    {
        private readonly ICashBoxService cashBoxService;
        private int currentPage = 0;

        public ObservableCollection<CashBoxProduct> DisplayedProducts { get; set; }

        public CashboxViewModel(ICashBoxService _cashBoxService)
        {
            cashBoxService = _cashBoxService;
            DisplayedProducts = new();
            UpdateDisplayedItems();
        }

        public void NextPage()
        {
            currentPage++;
            UpdateDisplayedItems();
        }

        public void PreviousPage()
        {
            currentPage--;
            UpdateDisplayedItems();
        }

        private void UpdateDisplayedItems()
        {
            DisplayedProducts.Clear();
            //DisplayedProducts = new(new CashBoxProduct[0], 0, 0);

            //int startIndex = currentPage * itemsPerPage;
            //int endIndex = Math.Min(startIndex + itemsPerPage, products.Count);
            PagedResult<CashBoxProduct> products = cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = 1 }, 1, null).Result;

            for (int i = 0; i < products.Items.Count; i++)
            {
                DisplayedProducts.Add(products.Items[i]);

            }
        }

    }
}
