using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Input;
using Cashbox = Supermarket.Core.Domain.CashBoxes.CashBox;

namespace Supermarket.Wpf.Manager.SupermarketCashboxes.Dialog
{
    public class SupermarketCashboxesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Cashbox, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public Cashbox? Cashbox { get; set; }
        public int CashboxId { get; set; }

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<Cashbox>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SupermarketCashboxesDialogViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            
            if (CashboxId != 0)
            {
                Cashbox = await _managerMenuService.GetCashboxToEdit(CashboxId);
            }
            else
            {
                Cashbox = new Cashbox
                {
                    Id = 0,
                    Name = "",
                    Code = "",
                    Notes = "",
                    SupermarketId = _loggedUserService.SupermarketId
                };
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Cashbox>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Cashbox != null)
            {
                if (CashboxId != 0)
                {
                    await _managerMenuService.EditCashbox(Cashbox);
                }
                else
                {
                    await _managerMenuService.AddCashbox(Cashbox);
                }
                ResultReceived?.Invoke(this, DialogResult<Cashbox>.Ok(Cashbox));
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(Cashbox?.Name) && ValidateInput.IsValidStringInput(Cashbox?.Code))
                return true;

            return false;
        }

        public async void SetParameters(int parameters)
        {
            CashboxId = parameters;
            await InitializeAsync();
        }

        
    }
}
