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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Wpf.Manager.SupermarketEmployees.Dialog
{
    public class ManagerMenuEmployeeDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<EmptyResult, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<EmptyResult>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public ManagerMenuEmployeeDetail? Employee { get; set; }
        
        private string? _employeePassword;
        public string? EmployeePassword
        {
            get => _employeePassword;
            set => SetProperty(ref _employeePassword, value);
        }

        private PossibleManagerForEmployee? _selectedManager;

        public PossibleManagerForEmployee? SelectedManager
        {
            get => _selectedManager;
            set => SetProperty(ref _selectedManager, value);
        }
        public ObservableCollection<PossibleManagerForEmployee> PossibleManagersForEmployee { get; } = new();

        public ManagerMenuEmployeeDialogViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }
        
        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Employee is null)
            {
                return;
            }
            
            var roles = GetRoles(Employee);

            if (Employee.Id == 0)
            {
                if (EmployeePassword is null)
                {
                    return;
                }

                await _managerMenuService.AddEmployee(new ManagerAddEmployee
                {
                    Id = Employee.Id,
                    Name = Employee.Name,
                    Surname = Employee.Surname,
                    Login = Employee.Login,
                    Password = EmployeePassword,
                    HireDate = Employee.HireDate,
                    SupermarketId = Employee.SupermarketId,
                    ManagerId = SelectedManager?.EmployeeId,
                    Roles = roles
                });
            }
            else
            {
                await _managerMenuService.EditEmployee(new ManagerEditEmployee
                {
                    Id = Employee.Id,
                    Name = Employee.Name,
                    Surname = Employee.Surname,
                    Login = Employee.Login,
                    NewPassword = EmployeePassword,
                    HireDate = Employee.HireDate,
                    ManagerId = SelectedManager?.EmployeeId,
                    Roles = roles
                });
            }

            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Ok(EmptyResult.Value));

        }

        private static HashSet<SupermarketEmployeeRole> GetRoles(ManagerMenuEmployeeDetail employeeDetail)
        {
            var roles = new HashSet<SupermarketEmployeeRole>();
            if (employeeDetail.IsCashier)
            {
                roles.Add(SupermarketEmployeeRole.Cashier);
            }

            if (employeeDetail.IsGoodsKeeper)
            {
                roles.Add(SupermarketEmployeeRole.GoodsKeeper);
            }

            if (employeeDetail.IsManager)
            {
                roles.Add(SupermarketEmployeeRole.Manager);
            }

            return roles;
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (Employee == null || (_loggedUserService.IsAdmin(out _) == false && SelectedManager == null))
            {
                return false;
            }

            if (Employee.Id == 0 && EmployeePassword is null)
            {
                return false;
            }

            return true;
        }

        public async void SetParameters(int parameters)
        {
            using var _ = new DelegateLoading(this);
            
            if (parameters != 0)
            {
                Employee = await _managerMenuService.GetEmployeeToEdit(parameters);
                SelectedManager = PossibleManagersForEmployee
                    .FirstOrDefault(x => x.EmployeeId == Employee.ManagerId);
            }
            else
            {
                Employee = new ManagerMenuEmployeeDetail
                {
                    Id = 0,
                    Login = "",
                    Name = "",
                    Surname = "",
                    HireDate = DateTime.Now,
                    SupermarketId = _loggedUserService.SupermarketId,
                    ManagerId = 0,
                    IsCashier = false,
                    IsManager = false,
                    IsGoodsKeeper = false
                };
            }
        }

        public async Task InitializeAsync()
        {
            if (!_loggedUserService.IsEmployee(out var employeeData))
            {
                throw new UiInconsistencyException($"Unregistered user is not allowed in {nameof(ManagerMenuEmployeeDialogViewModel)}");
            }

            var possibleManagers = await _managerMenuService.GetPossibleManagers(employeeData.Id, _loggedUserService.SupermarketId, new RecordsRange { PageNumber = 1, PageSize = 250});
            PossibleManagersForEmployee.Update(possibleManagers.Items);
        }
    }
}
