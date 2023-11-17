using Supermarket.Core.Domain.Common;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Wpf.Admin.Supermarkets.Dialog;

namespace Supermarket.Wpf.Admin.Supermarkets
{
    public class AdminSupermarketsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<AdminSupermarket>? _supermarkets;
        public ObservableCollection<AdminSupermarket> Supermarkets { get; set; }

        private AdminSupermarket? _selectedSupermarket;
        public AdminSupermarket? SelectedSupermarket
        {
            get => _selectedSupermarket;
            set
            {
                _selectedSupermarket = value;
                OnPropertyChanged(nameof(SelectedSupermarket));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Supermarkety";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminSupermarketsViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);

            Supermarkets = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            _supermarkets = await _adminMenuService.GetAllSupermarkets(new RecordsRange { PageSize = 500, PageNumber = 1 });

            Supermarkets.Clear();
            foreach (var supermarket in _supermarkets.Items)
            {
                Supermarkets.Add(supermarket);
            }
        }

        public async void Add(object? obj)
        {
            int selectedSupermarketId = 0;
            var result = await _dialogService.ShowAsync<SupermarketsDialogViewModel, Core.Domain.Supermarkets.Supermarket, int>(selectedSupermarketId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Edit(object? obj)
        {
            int selectedSupermarketId = SelectedSupermarket?.Id ?? 0;
            var result = await _dialogService.ShowAsync<SupermarketsDialogViewModel, Core.Domain.Supermarkets.Supermarket, int>(selectedSupermarketId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedSupermarket?.Address}");

            if (result.IsOk())
            {
                int selectedSupermarketId = SelectedSupermarket?.Id ?? 0;
                try
                {
                    await _adminMenuService.DeleteSupermarket(selectedSupermarketId);
                }
                catch (OperationCannotBeExecutedException)
                {
                    MessageBox.Show("Nelze smazat supermarket protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await InitializeAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedSupermarket != null;
        }
    }
}
