using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Suppliers;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Infrastructure.CashBoxes;
using Supermarket.Wpf.Admin.Suppliers.Dialog;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.Manager.SupermarketCashboxes.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.Admin.Suppliers
{
    public class AdminSuppliersViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<Supplier>? _suppliers;
        public ObservableCollection<Supplier> Suppliers { get; set; }

        private Supplier? _selectedSupplier;
        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Dodavatele";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminSuppliersViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(AddSupplier);
            EditCommand = new RelayCommand(EditSupplier, CanCallDialog);
            DeleteCommand = new RelayCommand(DeleteSupplier, CanCallDialog);

            Suppliers = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            _suppliers = await _adminMenuService.GetAllSuppliers(new RecordsRange { PageSize = 500, PageNumber = 1 });

            Suppliers.Clear();
            foreach(var supplier in _suppliers.Items)
            {
                Suppliers.Add(supplier);
            }

        }

        public async void AddSupplier(object? obj)
        {
            int selectedSupplierId = 0;
            var result = await _dialogService.ShowAsync<SuppliersDialogViewModel, Supplier, int>(selectedSupplierId);
            if (result.IsOk(out var cashBox))
            {
                await InitializeAsync();
            }
        }

        public async void EditSupplier(object? obj)
        {
            int selectedSupplierId = SelectedSupplier?.Id ?? 0;
            var result = await _dialogService.ShowAsync<SuppliersDialogViewModel, Supplier, int>(selectedSupplierId);
            if (result.IsOk(out var cashBox))
            {
                await InitializeAsync();
            }
        }

        public async void DeleteSupplier(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedSupplier?.Name}");

            if (result.IsOk())
            {
                int selectedSupplierId = SelectedSupplier?.Id ?? 0;
                await _adminMenuService.DeleteSupplier(selectedSupplierId);
                await InitializeAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedSupplier != null;
        }
    }
}
