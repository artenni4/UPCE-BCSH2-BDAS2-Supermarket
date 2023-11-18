using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
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

        private ManagerMenuEmployeeModel _employee = new();
        public ManagerMenuEmployeeModel Employee
        {
            get => _employee;
            set => SetProperty(ref _employee, value);
        }

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
            set
            {
                SetProperty(ref _selectedManager, value);
                Employee.ManagerId = value?.EmployeeId;
            }
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
            if (Employee?.Name is null || Employee.Surname is null || Employee.Login is null || Employee.HireDate is null)
            {
                return;
            }
            
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
                    HireDate = Employee.HireDate.Value,
                    RoleInfo = Employee.GetEmployeeRoleInfo(_loggedUserService.SupermarketId)
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
                    HireDate = Employee.HireDate.Value,
                    RoleInfo = Employee.GetEmployeeRoleInfo(_loggedUserService.SupermarketId)
                });
            }

            ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Ok(EmptyResult.Value));

        }

        private bool CanConfirmEdit(object? arg)
        {
            if ((_loggedUserService.IsAdmin(out _) == false && SelectedManager == null))
            {
                return false;
            }
            
            if (Employee.Name is null || Employee.Surname is null || Employee.Login is null || Employee.HireDate is null)
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
                var employeeToEdit = await _managerMenuService.GetEmployeeToEdit(parameters);
                Employee = ManagerMenuEmployeeModel.FromManagerMenuEmployeeDetail(employeeToEdit);
                SelectedManager = PossibleManagersForEmployee
                    .FirstOrDefault(x => x.EmployeeId == Employee.ManagerId);
            }
            else
            {
                Employee = new ManagerMenuEmployeeModel();
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
