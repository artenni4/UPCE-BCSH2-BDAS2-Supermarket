using Supermarket.Core.Domain.Common;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Admin.Regions.Dialog;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.Admin.Products.Dialog;
using Supermarket.Core.Domain.Products;

namespace Supermarket.Wpf.Admin.Products
{
    public class AdminProductsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<AdminProduct>? _products;
        public ObservableCollection<AdminProduct> Products { get; set; }

        private AdminProduct? _selectedProduct;
        public AdminProduct? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(Search));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public string TabHeader => "Zboží";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminProductsViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);
            SearchCommand = new RelayCommand(Search);

            Products = new();
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            _products = await _adminMenuService.GetAdminProducts(new RecordsRange { PageSize = 500, PageNumber = 1 });
            if (string.IsNullOrEmpty(SearchText))
            {
                Products.Clear();
                foreach (var region in _products.Items)
                {
                    Products.Add(region);
                }
            }
            else
            {
                Products.Clear();
                foreach (var region in _products.Items.Where(x => x.Name.Contains(SearchText)))
                {
                    Products.Add(region);
                }
            }
        }

        public async void Add(object? obj)
        {
            int selectedRegionId = 0;
            var result = await _dialogService.ShowAsync<ProductsDialogViewModel, Product, int>(selectedRegionId);
            if (result.IsOk(out var _))
            {
                await ActivateAsync();
            }
        }

        public async void Search(object? obj)
        {
            await ActivateAsync();
        }

        public async void Edit(object? obj)
        {
            int selectedRegionId = SelectedProduct?.Id ?? 0;
            var result = await _dialogService.ShowAsync<ProductsDialogViewModel, Product, int>(selectedRegionId);
            if (result.IsOk(out var _))
            {
                await ActivateAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedProduct?.Name}");

            if (result.IsOk())
            {
                int selectedRegionId = SelectedProduct?.Id ?? 0;
                try
                {
                    await _adminMenuService.DeleteProduct(selectedRegionId);
                }
                catch (ConstraintViolatedException)
                {
                    MessageBox.Show("Nelze smazat zboží protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await ActivateAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedProduct?.Name != null;
        }
    }
}
