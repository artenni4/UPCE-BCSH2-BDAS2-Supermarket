using Supermarket.Core.Domain.Employees;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Wpf.Manager.SupermarketEmployees.Dialog;
using Supermarket.Core.Domain.Common;
using System.Windows;

namespace Supermarket.Wpf.Manager.SupermarketEmployees
{
    public class SupermarketEmployeesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;

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
            if (!_loggedUserService.IsEmployee(out var employeeData))
            {
                throw new UiInconsistencyException($"Unauthorized user is not allowed in {nameof(SupermarketEmployeesViewModel)}");
            }
            
            var employees = await _managerMenuService.GetSupermarketEmployees(employeeData.Id, _loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });
            Employees.Update(employees.Items);
        }

        public async void AddEmployee(object? obj)
        {
            int selectedEmployeeId = 0;
            var result = await _dialogService.ShowAsync<ManagerMenuEmployeeDialogViewModel, int>(selectedEmployeeId);
            if (result.IsOk())
            {
                await InitializeAsync();
            }
        }

        public async void EditEmployee(object? obj)
        {
            int selectedEmployeeId = SelectedEmployee?.EmployeeId ?? 0;
            var result = await _dialogService.ShowAsync<ManagerMenuEmployeeDialogViewModel, int>(selectedEmployeeId);
            if (result.IsOk())
            {
                await InitializeAsync();
            }
        }

        public async void DeleteEmployee(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedEmployee?.Name} {SelectedEmployee?.Surname}");

            if (result.IsOk())
            {
                int selectedEmployeeId = SelectedEmployee?.EmployeeId ?? 0;
                try
                {
                    await _managerMenuService.DeleteEmployee(selectedEmployeeId);
                }
                catch (OperationCannotBeExecutedException)
                {
                    MessageBox.Show("Nelze smazat zaměstnance protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await InitializeAsync();
            }
        }

        private bool CanOpenDialog(object? arg)
        {
            return SelectedEmployee != null;
        }
    }
}
