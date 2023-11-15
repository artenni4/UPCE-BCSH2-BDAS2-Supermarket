using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketSales
{
    public class SupermarketSalesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        private PagedResult<ManagerMenuSale>? _sales;
        public ObservableCollection<ManagerMenuSale> Sales { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public ICommand SearchCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Prodeje";

        public SupermarketSalesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            SearchCommand = new RelayCommand(Search);
            Sales = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            DateFrom = DateTime.Today;
            DateTo = DateTime.Today.AddDays(1).AddMinutes(-1);
            await GetSales();
        }

        private async Task GetSales()
        {
            Sales.Clear();
            _sales = await _managerMenuService.GetSupermarketSales(_loggedUserService.SupermarketId, DateFrom, DateTo, new RecordsRange { PageSize = 500, PageNumber = 1 });

            foreach (var sale in _sales.Items)
            {
                Sales.Add(sale);
            }
        }

        public async void Search(object? obj)
        {
            await GetSales();
        }
    }
}
