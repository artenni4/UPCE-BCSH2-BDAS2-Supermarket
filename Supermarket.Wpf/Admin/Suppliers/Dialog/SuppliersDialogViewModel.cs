using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Input;

namespace Supermarket.Wpf.Admin.Suppliers.Dialog
{
    public class SuppliersDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Supplier, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public Supplier? Supplier { get; set; }
        public int SupplierId { get; set; }

        public event EventHandler<DialogResult<Supplier>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SuppliersDialogViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (SupplierId != 0)
            {
                Supplier = await _adminMenuService.GetSupplier(SupplierId);
            }
            else
            {
                Supplier = new Supplier
                {
                    Id = 0,
                    Name = ""
                };
            }
        }

        public async void SetParameters(int parameters)
        {
            SupplierId = parameters;
            await InitializeAsync();
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Supplier>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Supplier != null)
            {
                if (SupplierId != 0)
                {
                    await _adminMenuService.EditSupplier(Supplier);
                    ResultReceived?.Invoke(this, DialogResult<Supplier>.Ok(Supplier));
                }
                else
                {
                    await _adminMenuService.AddSupplier(Supplier);
                    ResultReceived?.Invoke(this, DialogResult<Supplier>.Ok(Supplier));
                }
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(Supplier?.Name))
            {
                return true;
            }
            else
                return false;
        }
    }
}
