using Supermarket.Core.Domain.Employees;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Wpf.Manager.SupermarketEmployees.Dialog;

namespace Supermarket.Wpf.Manager.SupermarketEmployees
{
    public class SupermarketEmployeesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;

        private PagedResult<ManagerMenuEmployee>? _employees;
        public ObservableCollection<ManagerMenuEmployee> Employees { get; set; }

        private ManagerMenuEmployee? _selectedEmployee;
        public ManagerMenuEmployee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Zaměstnanci";

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public SupermarketEmployeesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService, IDialogService dialogService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(AddEmployee);
            EditCommand = new RelayCommand(EditEmployee, CanOpenDialog);
            DeleteCommand = new RelayCommand(DeleteEmployee, CanOpenDialog);

            Employees = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            await GetEmployees();
        }

        private async Task GetEmployees()
        {
            _employees = await _managerMenuService.GetManagerEmployees(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });

            foreach(var emp in _employees.Items)
            {
                Employees.Add(emp);
            }
            
        }

        public async void AddEmployee(object? obj)
        {
            int selectedEmployeeId = 0;
            var result = await _dialogService.ShowAsync<ManagerMenuEmployeeDialogViewModel, Employee, int>(selectedEmployeeId);
            if (result.IsOk(out var a))
            {

            }
        }

        public async void EditEmployee(object? obj)
        {
            int selectedEmployeeId = SelectedEmployee?.EmployeeId ?? 0;
            var result = await _dialogService.ShowAsync<ManagerMenuEmployeeDialogViewModel, Employee, int>(selectedEmployeeId);
            if (result.IsOk(out var a))
            {

            }
        }

        public async void DeleteEmployee(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedEmployee?.Name} {SelectedEmployee?.Surname}");

            if (result.IsOk())
            {
                int selectedEmployeeId = SelectedEmployee?.EmployeeId ?? 0;
                await _managerMenuService.DeleteEmployee(selectedEmployeeId);
                await InitializeAsync();
            }
        }

        private bool CanOpenDialog(object? arg)
        {
            return SelectedEmployee != null;
        }
    }
}
