using Supermarket.Core.Domain.Common;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.Admin.Employees.Dialog;

namespace Supermarket.Wpf.Admin.Employees
{
    public class AdminEmployeesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<AdminEmployee>? _employees;
        public ObservableCollection<AdminEmployee> Employees { get; set; }

        private AdminEmployee? _selectedEmployee;
        public AdminEmployee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Zaměstnanci";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminEmployeesViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);

            Employees = new();
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            _employees = await _adminMenuService.GetAllEmployees(new RecordsRange { PageSize = 500, PageNumber = 1 });

            Employees.Clear();
            foreach (var supplier in _employees.Items)
            {
                Employees.Add(supplier);
            }

        }

        public async void Add(object? obj)
        {
            int selectedEmployeeId = 0;
            var result = await _dialogService.ShowAsync<EmployeesDialogViewModel, EmptyResult, int>(selectedEmployeeId);
            if (result.IsOk(out var _))
            {
                await ActivateAsync();
            }
        }

        public async void Edit(object? obj)
        {
            int selectedEmployeeId = SelectedEmployee?.Id ?? 0;
            var result = await _dialogService.ShowAsync<EmployeesDialogViewModel, EmptyResult, int>(selectedEmployeeId);
            if (result.IsOk(out var _))
            {
                await ActivateAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedEmployee?.Name}");

            if (result.IsOk())
            {
                int selectedEmployeeId = SelectedEmployee?.Id ?? 0;
                try
                {
                    await _adminMenuService.DeleteEmployee(selectedEmployeeId);
                }
                catch (ConstraintViolatedException)
                {
                    MessageBox.Show("Nelze smazat zaměstnance protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await ActivateAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedEmployee != null;
        }
    }
}
