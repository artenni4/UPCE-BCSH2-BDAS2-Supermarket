using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Manager.SupermarketEmployees.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Wpf.Admin.Employees.Dialog
{
    public class EmployeesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<EmptyResult, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        private AdminMenuEmployeeModel _employee = new();
        public AdminMenuEmployeeModel Employee
        {
            get => _employee;
            set => SetProperty(ref _employee, value);
        }

        public int EmployeeId { get; set; }
        public ObservableCollection<PossibleManagerForEmployee> Managers { get; } = new();
        
        private PossibleManagerForEmployee? _selectedManager;
        public PossibleManagerForEmployee? SelectedManager
        {
            get => _selectedManager;
            set => SetProperty(ref _selectedManager, value);
        }

        public ObservableCollection<AdminMenuSupermarket> Supermarkets { get; } = new ObservableCollection<AdminMenuSupermarket>();
        private AdminMenuSupermarket? _selectedSupermarket;
        public AdminMenuSupermarket? SelectedSupermarket
        {
            get => _selectedSupermarket;
            set
            {
                SetProperty(ref _selectedSupermarket, value);

                _ = Task.Run(async () =>
                {
                    if (value is not null)
                    {
                        var managers = await _adminMenuService.GetPossibleManagers(value.Id, new RecordsRange { PageSize = 200, PageNumber = 1 });
                        Managers.Update(managers.Items);
                    }
                    else
                    {
                        Managers.Clear();
                    }
                });
            }
        }

        public event EventHandler<DialogResult<EmptyResult>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public EmployeesDialogViewModel(IAdminMenuService adminMenuService, ILoggedUserService loggedUserService)
        {
            _adminMenuService = adminMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        private string? _employeePassword;

        public string? EmployeePassword
        {
            get => _employeePassword;
            set => SetProperty(ref _employeePassword, value);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            var supermarkets = await _adminMenuService.GetAllSupermarkets(new RecordsRange { PageSize = 250, PageNumber = 1 });
            Supermarkets.Update(supermarkets.Items);
            if (EmployeeId != 0)
            {
                var employeeToEdit = await _adminMenuService.GetEmployeeToEdit(EmployeeId);
                Employee = AdminMenuEmployeeModel.FromAdminEmployeeDetail(employeeToEdit);
                if (employeeToEdit.RoleInfo is SupermarketEmployee supermarketEmployee)
                {
                    SelectedSupermarket = Supermarkets.FirstOrDefault(x => x.Id == supermarketEmployee.SupermarketId);
                }
            }
            else
            {
                Employee = new AdminMenuEmployeeModel();
            }
        }

        public async void SetParameters(int parameters)
        {
            EmployeeId = parameters;
            await InitializeAsync();
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Employee.Name is null || Employee.Surname is null || Employee.Login is null || Employee.HireDate is null)
            {
                return;
            }

            if (Employee.Id == 0)
            {
                if (EmployeePassword is null)
                {
                    return;
                }
                var addEmp = new AdminAddEmployee
                {
                    Id = Employee.Id,
                    Name = Employee.Name,
                    Surname = Employee.Surname,
                    Login = Employee.Login,
                    Password = EmployeePassword,
                    HireDate = Employee.HireDate.Value,
                    RoleInfo = Employee.GetEmployeeRoleInfo(SelectedSupermarket?.Id, SelectedManager?.EmployeeId),
                    PersonalNumber = Employee.PersonalNumber
                };

                await _adminMenuService.AddEmployee(addEmp);
            }
            else
            {
                var editEmp = new AdminEditEmployee
                {
                    Id = Employee.Id,
                    Name = Employee.Name,
                    Surname = Employee.Surname,
                    Login = Employee.Login,
                    NewPassword = EmployeePassword,
                    HireDate = Employee.HireDate.Value,
                    RoleInfo = Employee.GetEmployeeRoleInfo(SelectedSupermarket?.Id, SelectedManager?.EmployeeId),
                    PersonalNumber = Employee.PersonalNumber
                };

                await _adminMenuService.EditEmployee(editEmp);

            }

            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Ok(EmptyResult.Value));
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (Employee.Name is null || Employee.Surname is null || Employee.Login is null || Employee.HireDate is null)
            {
                return false;
            }
            
            if (!Employee.IsAdmin && SelectedSupermarket is null)
            {
                return false;
            }

            if (Employee.Id == 0 && EmployeePassword is null)
            {
                return false;
            }

            return true;
        }
    }
}
