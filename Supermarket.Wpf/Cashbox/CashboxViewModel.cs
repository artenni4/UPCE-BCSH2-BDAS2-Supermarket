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
        private int _currentPage = 1;

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value < 1)
                {
                    _currentPage = 1;
                }
                else if (value > 200)
                {
                    _currentPage = 200;
                }
                else
                {
                    _currentPage = value;
                }
            }
        }

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
            CurrentPage++;
            UpdateDisplayedItems();
        }

        public void PreviousPage(object? obj)
        {
            CurrentPage--;
            UpdateDisplayedItems();
        }

        private void UpdateDisplayedItems()
        {
            PagedResult<CashBoxProduct> products = _cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = CurrentPage }, 1, null).Result;
            if (products.Items.Count != 0) 
            {
                DisplayedProducts.Clear();

                for (int i = 0; i < products.Items.Count; i++)
                {
                    DisplayedProducts.Add(products.Items[i]);

                }
            }
            else // do not increment page value if the end of the list is reached
                CurrentPage--;
        }

    }
}
