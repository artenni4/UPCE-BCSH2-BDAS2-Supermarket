using System;
using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Wpf.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Supermarket.Domain.Auth.LoggedEmployees;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Cashbox
{
    public class CashboxViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly ILoggedUserService _loggedUserService;
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
        public ObservableCollection<SelectedProductModel> SelectedProducts { get; set; }

        private bool _isAssistantLoggedIn;
        public bool IsAssistantLoggedIn
        {
            get => _isAssistantLoggedIn;
            set => SetProperty(ref _isAssistantLoggedIn, value);
        }
        
        public bool IsCustomerCashBox { get; private set; }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand CategoryButtonClickCommand { get; }
        public ICommand ProductClickCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand InviteAssistantCommand { get; }
        public ICommand AssistantExitCommand { get; }

        public CashboxViewModel(ICashBoxService cashBoxService, IDialogService dialogService, ILoggedUserService loggedUserService)
        {
            _cashBoxService = cashBoxService;
            _dialogService = dialogService;
            _loggedUserService = loggedUserService;
            
            DisplayedProducts = new ObservableCollection<CashBoxProduct>();
            Categories = new ObservableCollection<CashBoxProductCategory>();
            SelectedProducts = new ObservableCollection<SelectedProductModel>();

            IsCustomerCashBox = loggedUserService.IsLoggedCustomer();

            NextPageCommand = new RelayCommand(NextPage, _ => products?.HasNext == true);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => products?.HasPrevious == true);
            CategoryButtonClickCommand = new RelayCommand(CategoryButtonClick);
            ProductClickCommand = new RelayCommand(ProductClick);
            InviteAssistantCommand = new RelayCommand(InviteAssistant);
            AssistantExitCommand = new RelayCommand(AssistantExit);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
        }

        private void RemoveProduct(object? obj)
        {
            if (obj is not SelectedProductModel item)
            {
                return;
            }
            
            SelectedProducts.Remove(item);
        }

        private void AssistantExit(object? obj)
        {
            _loggedUserService.ResetLoggedEmployee();
            IsAssistantLoggedIn = false;
        }

        private async void InviteAssistant(object? obj)
        {
            var result = await _dialogService.ShowForResultAsync<LoginAssistantViewModel, DialogResult<ILoggedEmployee>, EmptyParameters>(EmptyParameters.Value);
            if (result.IsOk(out var loggedEmployee))
            {
                _loggedUserService.SetLoggedEmployee(loggedEmployee);
                IsAssistantLoggedIn = true;
            }
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

        private async void ProductClick(object? obj)
        {
            if (obj is not CashBoxProduct selectedProduct)
            {
                return;
            }
            
            decimal count = 1;
            if (selectedProduct.IsByWeight)
            {
                var result = await _dialogService
                    .ShowForResultAsync<ProductCountInputViewModel, DialogResult<decimal>, EmptyParameters>(EmptyParameters.Value);

                if (! result.IsOk(out count))
                {
                    return;
                }
            }
            
            SelectedProducts.Add(new SelectedProductModel { CashBoxProduct = selectedProduct, Count = count });
        }
    }
}
