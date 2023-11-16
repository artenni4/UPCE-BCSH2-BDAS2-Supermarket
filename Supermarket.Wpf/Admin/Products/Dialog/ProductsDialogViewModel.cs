using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.Wpf.Admin.Products.Dialog
{
    public class ProductsDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Product, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public ObservableCollection<MeasureUnit> MeasureUnits { get; } = new ObservableCollection<MeasureUnit>();
        private PagedResult<Supplier>? _suppliers;
        public ObservableCollection<Supplier> Suppliers { get; } = new();
        private PagedResult<ProductCategory>? _categories;
        public ObservableCollection<ProductCategory> Categories { get; } = new();

        private bool _isByWeight;
        public bool IsByWeight
        {
            get => _isByWeight;
            set
            {
                if (_isByWeight != value)
                {
                    _isByWeight = value;
                    OnPropertyChanged(nameof(IsByWeight));
                    UpdateMeasureUnitAvailability();
                }
            }
        }

        private MeasureUnit? _selectedMeasureUnit;
        public MeasureUnit? SelectedMeasureUnit
        {
            get => _selectedMeasureUnit;
            set
            {
                if (_selectedMeasureUnit != value)
                {
                    _selectedMeasureUnit = value;
                    OnPropertyChanged(nameof(SelectedMeasureUnit));
                }
            }
        }

        private Supplier? _selectedSupplier;
        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
                }
            }
        }

        private ProductCategory? _selectedCategory;
        public ProductCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public bool IsMeasureUnitEnabled => IsByWeight;

        public event EventHandler<DialogResult<Product>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ProductsDialogViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (ProductId != 0)
            {
                Product = await _adminMenuService.GetProduct(ProductId);
            }
            else
            {
                Product = new Product
                {
                    Id = 0,
                    Name = "",
                    Barcode = "",
                    ByWeight = false,
                    Price = 0,
                    Description = "",
                    MeasureUnit = MeasureUnit.Kilogram,
                    ProductCategoryId = 0,
                    SupplierId = 0,
                    Weight = 0 
                };
            }

            if (Product != null && ProductId != 0)
            {
                SelectedMeasureUnit = Product.MeasureUnit;
                SelectedSupplier = await _adminMenuService.GetSupplier(Product.SupplierId);
                SelectedCategory = await _adminMenuService.GetCategory(Product.ProductCategoryId);
            }

            if (ProductId == 0)
            {
                SelectedMeasureUnit = MeasureUnit.Piece;
            }

            GetMeasureUnits();
            GetSuppliers();
            GetCategories();
        }

        public async void SetParameters(int parameters)
        {
            ProductId = parameters;
            await InitializeAsync();
        }

        private void GetMeasureUnits()
        {
            MeasureUnits.Clear();
            foreach (var mUnit in MeasureUnit.Values)
            {
                MeasureUnits.Add(mUnit);
            }
        }

        private async void GetSuppliers()
        {
            _suppliers = await _adminMenuService.GetAllSuppliers(new RecordsRange { PageSize = 300, PageNumber= 1 });
            
            Suppliers.Clear();
            foreach(var supplier in _suppliers.Items)
            {
                Suppliers.Add(supplier);
            }
        }

        private async void GetCategories()
        {
            _categories = await _adminMenuService.GetAllCategories(new RecordsRange { PageSize = 100, PageNumber = 1 });

            Categories.Clear();
            foreach(var category in _categories.Items)
            {
                Categories.Add(category);
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Product>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Product != null && SelectedMeasureUnit != null && SelectedSupplier != null && SelectedCategory != null)
            {
                var saveProduct = new Product 
                {
                    Id = Product.Id, 
                    Name = Product.Name,
                    Barcode = Product.Barcode,
                    ByWeight = Product.ByWeight,
                    Description = Product.Description,
                    MeasureUnit = SelectedMeasureUnit,
                    Price = Product.Price,
                    ProductCategoryId = SelectedCategory.Id,
                    SupplierId = SelectedSupplier.Id,
                    Weight = Product.Weight
                };
                if (ProductId != 0)
                {
                    await _adminMenuService.EditProduct(saveProduct);
                    ResultReceived?.Invoke(this, DialogResult<Product>.Ok(saveProduct));
                }
                else
                {
                    await _adminMenuService.AddProduct(saveProduct);
                    ResultReceived?.Invoke(this, DialogResult<Product>.Ok(saveProduct));
                }
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(Product?.Name))
            {
                return true;
            }
            else
                return false;
        }

        private void UpdateMeasureUnitAvailability()
        {
            if (!IsByWeight)
            {
                SelectedMeasureUnit = MeasureUnit.Piece;
            }
            OnPropertyChanged(nameof(IsMeasureUnitEnabled));
        }
    }
}
