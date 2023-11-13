using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketEmployees.Dialog
{
    public class ManagerMenuEmployeeDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Employee>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<Employee>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public Employee? Employee { get; set; }

        public ManagerMenuEmployeeDialogViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Employee>.Cancel());
        }

        private void ConfirmEdit(object? obj)
        {


            //var result = new Employee { };
            //ResultReceived?.Invoke(this, DialogResult<Employee>.Ok(result));

        }

        private bool CanConfirmEdit(object? arg)
        {
            return false;
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            Employee = await _managerMenuService.GetEmployeeToEdit(1);
        }
    }
}
