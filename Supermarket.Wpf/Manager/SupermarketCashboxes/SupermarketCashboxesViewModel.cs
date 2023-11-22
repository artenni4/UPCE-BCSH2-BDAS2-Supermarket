using Supermarket.Core.Domain.Common;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Manager.SupermarketCashboxes.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Cashbox = Supermarket.Core.Domain.CashBoxes.CashBox;

namespace Supermarket.Wpf.Manager.SupermarketCashboxes
{
    public class SupermarketCashboxesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;

        private PagedResult<ManagerMenuCashbox>? cashboxes;
        public ObservableCollection<ManagerMenuCashbox> Cashboxes { get; set; }

        private ManagerMenuCashbox? _selectedCashbox;
        public ManagerMenuCashbox? SelectedCashbox
        {
            get => _selectedCashbox;
            set
            {
                _selectedCashbox = value;
                OnPropertyChanged(nameof(SelectedCashbox));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Pokladny";

        public SupermarketCashboxesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService, IDialogService dialogService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(AddCashbox);
            EditCommand = new RelayCommand(EditCashbox, CanCallDialog);
            DeleteCommand = new RelayCommand(DeleteCashbox, CanCallDialog);

            Cashboxes = new();
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            await GetCashboxes();
        }

        private async Task GetCashboxes()
        {
            cashboxes = await _managerMenuService.GetSupermarketCashboxes(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 50, PageNumber = 1 });

            Cashboxes.Clear();
            foreach (var cb in cashboxes.Items)
            {
                Cashboxes.Add(cb);
            }
        }

        public async void AddCashbox(object? obj)
        {
            int selectedCashboxId = 0;
            var result = await _dialogService.ShowAsync<SupermarketCashboxesDialogViewModel, Cashbox, int>(selectedCashboxId);
            if (result.IsOk(out var cashBox))
            {
                await ActivateAsync();
            }
        }

        public async void EditCashbox(object? obj)
        {
            int selectedCashboxId = SelectedCashbox?.Id ?? 0;
            var result = await _dialogService.ShowAsync<SupermarketCashboxesDialogViewModel, Cashbox, int>(selectedCashboxId);
            if (result.IsOk(out var cashBox))
            {
                await ActivateAsync();
            }
        }

        public async void DeleteCashbox(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedCashbox?.Name}");

            if (result.IsOk())
            {
                int selectedCashboxId = SelectedCashbox?.Id ?? 0;
                try
                {
                    await _managerMenuService.DeleteCashbox(selectedCashboxId);
                }
                catch (ConstraintViolatedException)
                {
                    MessageBox.Show("Nelze odstranit pokladnu, která již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await ActivateAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedCashbox != null;
        }

    }
}
