using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketEmployees.Dialog
{
    public class ManagerMenuEmployeeDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Employee, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<Employee>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ManagerMenuEmployeeDetail? Employee { get; set; }
        public int EmployeeId { get; set; }
        public bool IsCashier { get; set; }
        public bool IsGoodsKeeper { get; set; }
        public bool IsManager { get; set; }

        public ManagerMenuEmployeeDialogViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            Employee = await _managerMenuService.GetEmployeeToEdit(1);
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Employee>.Cancel());
        }

        private void ConfirmEdit(object? obj)
        {
            //var emp = new Employee
            //{
            //    Id = Employee.Id,

            //};
            //await _adminMenuService.SaveEmplo

            //var result = new Employee { };
            //ResultReceived?.Invoke(this, DialogResult<Employee>.Ok(result));

        }

        private bool CanConfirmEdit(object? arg)
        {
            if (Employee != null)
                return true;

            return false;
        }

        public async void SetParameters(int parameters)
        {
            EmployeeId = parameters;
            await InitializeAsync();
        }
    }
}
