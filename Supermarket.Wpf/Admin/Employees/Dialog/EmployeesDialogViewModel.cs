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

namespace Supermarket.Wpf.Admin.Employees.Dialog
{
    public class EmployeesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<EmptyResult, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public AdminEmployeeDetail? Employee { get; set; }
        public int EmployeeId { get; set; }
        public ObservableCollection<PossibleManagerForEmployee> Managers { get; } = new ObservableCollection<PossibleManagerForEmployee>();
        private PossibleManagerForEmployee? _selectedManager;
        public PossibleManagerForEmployee? SelectedManager
        {
            get => _selectedManager;
            set => SetProperty(ref _selectedManager, value);
        }

        public ObservableCollection<AdminSupermarket> Supermarkets { get; } = new ObservableCollection<AdminSupermarket>();
        private AdminSupermarket? _selectedSupermarket;
        public AdminSupermarket? SelectedSupermarket
        {
            get => _selectedSupermarket;
            set
            {
                SetProperty(ref _selectedSupermarket, value);
                GetPossibleManagers();
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

            if (EmployeeId != 0)
            {
                Employee = await _adminMenuService.GetEmployeeToEdit(EmployeeId);
            }
            else
            {
                Employee = new AdminEmployeeDetail
                {
                    Id = 0,
                    Login = "",
                    ManagerId = 0,
                    Name = "",
                    SupermarketId = 0,
                    Surname = "",
                    HireDate = DateTime.Now,
                    IsAdmin = false,
                    IsCashier = false,
                    IsGoodsKeeper = false,
                    IsManager = false,
                };
            }

            GetSupermarkets();
        }

        public async void SetParameters(int parameters)
        {
            EmployeeId = parameters;
            await InitializeAsync();
        }

        private async void GetSupermarkets()
        {
            var supermarkets = await _adminMenuService.GetAllSupermarkets(new RecordsRange { PageSize = 250, PageNumber = 1 });
            Supermarkets.Update(supermarkets.Items);

            if (Employee != null && EmployeeId != 0) 
            {
                SelectedSupermarket = Supermarkets.FirstOrDefault(x => x.Id == Employee.SupermarketId);
            }
        }

        private async void GetPossibleManagers()
        {
            if (SelectedSupermarket!= null)
            {
                var managers = await _adminMenuService.GetPossibleManagers(SelectedSupermarket.Id, new RecordsRange { PageSize = 200, PageNumber = 1 });
                Managers.Update(managers.Items);

                if (Employee != null && EmployeeId != 0)
                {
                    SelectedManager = Managers.FirstOrDefault(x => x.EmployeeId == Employee.ManagerId);
                }
            }
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
                var addEmp = new AdminAddEmployee
                {
                    Id = Employee.Id,
                    Name = Employee.Name,
                    Surname = Employee.Surname,
                    Login = Employee.Login,
                    Password = EmployeePassword,
                    HireDate = Employee.HireDate,
                    SupermarketId = SelectedSupermarket?.Id,
                    ManagerId = SelectedManager?.EmployeeId,
                    Roles = roles
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
                    HireDate = Employee.HireDate,
                    ManagerId = SelectedManager?.EmployeeId,
                    Roles = roles,
                    SupermarketId = SelectedSupermarket?.Id
                };

                await _adminMenuService.EditEmployee(editEmp);

            }

            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Ok(EmptyResult.Value));
        }

        private static HashSet<SupermarketEmployeeRole> GetRoles(AdminEmployeeDetail employeeDetail)
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

            if (employeeDetail.IsAdmin)
            {
                roles.Add(SupermarketEmployeeRole.Admin);
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
    }
}
