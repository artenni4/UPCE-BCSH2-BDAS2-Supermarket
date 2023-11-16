using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Input;

namespace Supermarket.Wpf.Admin.ProductCategories.Dialog
{
    class CategoriesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<ProductCategory, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public ProductCategory? Category { get; set; }
        public int CategoryId { get; set; }

        public event EventHandler<DialogResult<ProductCategory>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public CategoriesDialogViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (CategoryId != 0)
            {
                Category = await _adminMenuService.GetCategory(CategoryId);
            }
            else
            {
                Category = new ProductCategory
                {
                    Id = 0,
                    Name = "",
                    Description = ""
                };
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<ProductCategory>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Category != null)
            {
                if (CategoryId != 0)
                {
                    await _adminMenuService.EditCategory(Category);
                    ResultReceived?.Invoke(this, DialogResult<ProductCategory>.Ok(Category));
                }
                else
                {
                    await _adminMenuService.AddCategory(Category);
                    ResultReceived?.Invoke(this, DialogResult<ProductCategory>.Ok(Category));
                }
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(Category?.Name))
            {
                return true;
            }
            else
                return false;
        }

        public async void SetParameters(int parameters)
        {
            CategoryId = parameters;
            await InitializeAsync();
        }
    }
}
