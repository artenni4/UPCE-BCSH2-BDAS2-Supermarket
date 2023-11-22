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
        private PagedResult<Supplier>? _suppliers;
        public ObservableCollection<Supplier> Suppliers { get; } = new();
        private PagedResult<ProductCategory>? _categories;
        public ObservableCollection<ProductCategory> Categories { get; } = new();
        public static ObservableCollection<MeasureUnit> MeasureUnits { get; set; } = new ObservableCollection<MeasureUnit>()
        {
            MeasureUnit.Kilogram,
            MeasureUnit.Gram,
            MeasureUnit.Litre,
            MeasureUnit.Millilitre,
            MeasureUnit.Meter,
            MeasureUnit.Piece
        };


        private bool _byWeight;
        public bool ByWeight
        {
            get => _byWeight;
            set
            {
                if (_byWeight != value)
                {
                    _byWeight = value;
                    OnPropertyChanged(nameof(ByWeight));
                    UpdateMeasureUnits();
                    IsMeasureUnitEnabled = _byWeight;
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

        private bool _isMeasureUnitEnabled;
        public bool IsMeasureUnitEnabled
        {
            get => _isMeasureUnitEnabled;
            set
            {
                if (_isMeasureUnitEnabled != value)
                {
                    _isMeasureUnitEnabled = value;
                    OnPropertyChanged(nameof(IsMeasureUnitEnabled));
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
                    MeasureUnit = MeasureUnit.Piece,
                    ProductCategoryId = 0,
                    SupplierId = 0
                };
            }

            if (Product?.Name != string.Empty)
            {
                if (Product != null)
                {
                    ByWeight = Product.ByWeight;
                }
            }
            else
            {
                ByWeight = false;
                MeasureUnits = new ObservableCollection<MeasureUnit>()
                {
                    MeasureUnit.Piece
                };
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

        private void UpdateMeasureUnits()
        {
            MeasureUnits.Clear();
            if (ByWeight)
            {
                MeasureUnits.Add(MeasureUnit.Kilogram);
                MeasureUnits.Add(MeasureUnit.Gram);
                MeasureUnits.Add(MeasureUnit.Litre);
                MeasureUnits.Add(MeasureUnit.Millilitre);
                MeasureUnits.Add(MeasureUnit.Meter);
                SelectedMeasureUnit = MeasureUnits.FirstOrDefault();
            }
            else
            {
                MeasureUnits.Add(MeasureUnit.Piece);
                SelectedMeasureUnit = MeasureUnits.FirstOrDefault();
            }

            OnPropertyChanged(nameof(SelectedMeasureUnit));
            OnPropertyChanged(nameof(IsMeasureUnitEnabled));
        }


        private void GetMeasureUnits()
        {
            if (ProductId == 0)
            {
                SelectedMeasureUnit = MeasureUnit.Piece;
            }

            if (Product != null && ProductId != 0)
            {
                SelectedMeasureUnit = MeasureUnits.FirstOrDefault(s => s.Name == Product.MeasureUnit.Name);
            }
        }

        private async void GetSuppliers()
        {
            _suppliers = await _adminMenuService.GetAllSuppliers(new RecordsRange { PageSize = 300, PageNumber = 1 });

            Suppliers.Clear();
            foreach (var supplier in _suppliers.Items)
            {
                Suppliers.Add(supplier);
            }

            if (Product != null && ProductId != 0)
            {
                SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == Product.SupplierId);
            }
        }

        private async void GetCategories()
        {
            _categories = await _adminMenuService.GetAllCategories(new RecordsRange { PageSize = 100, PageNumber = 1 });

            Categories.Clear();
            foreach (var category in _categories.Items)
            {
                Categories.Add(category);
            }

            if (Product != null && ProductId != 0)
            {
                SelectedCategory = Categories.FirstOrDefault(s => s.Id == Product.ProductCategoryId);
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
                    ByWeight = ByWeight,
                    Description = Product.Description,
                    MeasureUnit = SelectedMeasureUnit,
                    Price = Product.Price,
                    ProductCategoryId = SelectedCategory.Id,
                    SupplierId = SelectedSupplier.Id,
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
            if (ValidateInput.IsValidStringInput(Product?.Name) && Product != null && SelectedMeasureUnit != null && SelectedSupplier != null && SelectedCategory != null && Product.Price > 0)
            {
                return true;
            }
            else
                return false;
        }

    }
}
