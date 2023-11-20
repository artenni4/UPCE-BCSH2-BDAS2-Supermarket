using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Admin.Suppliers.Dialog;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Wpf.Admin.ProductCategories.Dialog;

namespace Supermarket.Wpf.Admin.ProductCategories
{
    public class AdminMenuCategoriesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<ProductCategory>? _categories;
        public ObservableCollection<ProductCategory> Categories { get; set; }

        private ProductCategory? _selectedCategory;
        public ProductCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Druhy zboží";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminMenuCategoriesViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);

            Categories = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            _categories = await _adminMenuService.GetAllCategories(new RecordsRange { PageSize = 500, PageNumber = 1 });

            Categories.Clear();
            foreach (var supplier in _categories.Items)
            {
                Categories.Add(supplier);
            }
        }

        public async void Add(object? obj)
        {
            int selectedCategoryId = 0;
            var result = await _dialogService.ShowAsync<CategoriesDialogViewModel, ProductCategory, int>(selectedCategoryId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Edit(object? obj)
        {
            int selectedCategoryId = SelectedCategory?.Id ?? 0;
            var result = await _dialogService.ShowAsync<CategoriesDialogViewModel, ProductCategory, int>(selectedCategoryId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedCategory?.Name}");

            if (result.IsOk())
            {
                int selectedCategoryId = SelectedCategory?.Id ?? 0;
                try
                {
                    await _adminMenuService.DeleteCategory(selectedCategoryId);
                }
                catch (ConstraintViolatedException)
                {
                    MessageBox.Show("Nelze smazat druh zboží protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await InitializeAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedCategory?.Name != null;
        }
    }
}
