using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.CashBox;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.CashBox
{
    public class CashBoxViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly ICashBoxService _cashBoxService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private int _currentPage = 1;
        private int? _categoryId;
        private int? _cashBoxId;

        private PagedResult<CashBoxProduct>? _products;
        private PagedResult<CashBoxProductCategory>? _categories;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ObservableCollection<CashBoxProduct> DisplayedProducts { get; } = new();
        public ObservableCollection<CashBoxProductCategory> Categories { get; } = new();
        public ObservableCollection<SelectedProductModel> SelectedProducts { get; } = new();

        private bool _isAssistantLoggedIn;
        public bool IsAssistantLoggedIn
        {
            get => _isAssistantLoggedIn;
            set => SetProperty(ref _isAssistantLoggedIn, value);
        }
        
        public bool IsCustomerCashBox { get; private set; }

        public ICommand NextPageCommand { get; } 
        public ICommand PreviousPageCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }
        public ICommand InviteAssistantCommand { get; }
        public ICommand AssistantExitCommand { get; }
        public ICommand ClearProductsCommand { get; }

        public CashBoxViewModel(ICashBoxService cashBoxService, IDialogService dialogService, ILoggedUserService loggedUserService, INavigationService navigationService)
        {
            _cashBoxService = cashBoxService;
            _dialogService = dialogService;
            _loggedUserService = loggedUserService;
            _navigationService = navigationService;

            IsCustomerCashBox = loggedUserService.IsCustomer;

            NextPageCommand = new RelayCommand(NextPage, _ => _products?.HasNext == true);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => _products?.HasPrevious == true);
            SelectCategoryCommand = new RelayCommand(SelectCategory, CanSelectCategory);
            AddProductCommand = new RelayCommand(AddProduct);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
            InviteAssistantCommand = new RelayCommand(InviteAssistant);
            AssistantExitCommand = new RelayCommand(AssistantExit);
            ClearProductsCommand = new RelayCommand(ClearProducts, _ => SelectedProducts.Any());
        }
        
        public async Task InitializeAsync()
        {
            var cashBoxes = await _cashBoxService
                .GetCashBoxesAsync(_loggedUserService.SupermarketId, new RecordsRange { PageNumber = 1, PageSize = 100 });
            var dialogResult = await _dialogService
                .ShowDropDownDialogAsync("ZVOLTE POKLADNU", nameof(SupermarketCashBox.Code), cashBoxes.Items);

            if (dialogResult.IsOk(out var cashBox))
            {
                _cashBoxId = cashBox.Id;
            }
            else
            {
                await _navigationService.NavigateToAsync(ApplicationView.Login);
                return;
            }
            
            using var _ = new DelegateLoading(this);
            
            _categories = await _cashBoxService.GetCategoriesAsync(_loggedUserService.SupermarketId, new RecordsRange { PageNumber = 1, PageSize = 10 });
            _categoryId = _categories.Items.FirstOrDefault()?.CategoryId;
            Categories.AddRange(_categories.Items);
            
            await UpdateDisplayedItems();
        }

        private bool CanSelectCategory(object? arg)
        {
            if (arg is not CashBoxProductCategory productCategory)
            {
                return false;
            }

            return productCategory.CategoryId != _categoryId;
        }

        private async void ClearProducts(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync("Provedením této akce zrušite celý prodej");

            if (result.IsOk())
            {
                SelectedProducts.Clear();
            }
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
            _loggedUserService.UnsetUser();
            IsAssistantLoggedIn = false;
        }

        private async void InviteAssistant(object? obj)
        {
            var result = await _dialogService.ShowAsync<LoginAssistantViewModel, LoggedSupermarketEmployee>();
            if (result.IsOk(out var loggedEmployee))
            {
                _loggedUserService.SetSupermarketEmployee(loggedEmployee);
                IsAssistantLoggedIn = true;
            }
        }

        private async void NextPage(object? obj)
        {
            using var _ = new DelegateLoading(this);
            
            _currentPage++;
            await UpdateDisplayedItems();
        }

        private async void PreviousPage(object? obj)
        {
            using var _ = new DelegateLoading(this);
            
            _currentPage--;
            await UpdateDisplayedItems();
        }

        private async Task UpdateDisplayedItems()
        {
            if (_categoryId.HasValue == false)
            {
                return;
            }
            
            _products = await _cashBoxService.GetProductsAsync(1, new RecordsRange { PageSize = 10, PageNumber = _currentPage }, _categoryId.Value, null);
            DisplayedProducts.Update(_products.Items);
        }

        private async void SelectCategory(object? obj)
        {
            using var _ = new DelegateLoading(this);
            
            if (obj is CashBoxProductCategory selectedCategory)
            {
                _currentPage = 1;
                _categoryId = selectedCategory.CategoryId;
                await UpdateDisplayedItems();
            }
        }

        private async void AddProduct(object? obj)
        {
            if (obj is not CashBoxProduct selectedProduct)
            {
                return;
            }
            
            decimal count = 1;
            if (selectedProduct.IsByWeight)
            {
                var dialogResult = await _dialogService.ShowInputDialogAsync<decimal>(title: "POČET", inputLabel: selectedProduct.MeasureUnit);

                if (! dialogResult.IsOk(out count))
                {
                    return;
                }
            }
            
            SelectedProducts.Add(new SelectedProductModel { CashBoxProduct = selectedProduct, Count = count });
        }
    }
}
