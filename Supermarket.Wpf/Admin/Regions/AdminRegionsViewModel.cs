using Supermarket.Core.Domain.Common;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Core.Domain.Regions;
using Supermarket.Wpf.Admin.Regions.Dialog;

namespace Supermarket.Wpf.Admin.Regions
{
    public class AdminRegionsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;
        private readonly IDialogService _dialogService;

        private PagedResult<Region>? _regions;
        public ObservableCollection<Region> Regions { get; set; }

        private Region? _selectedRegion;
        public Region? SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                _selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TabHeader => "Regiony";
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public AdminRegionsViewModel(IAdminMenuService adminMenuService, IDialogService dialogService)
        {
            _adminMenuService = adminMenuService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);

            Regions = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            _regions = await _adminMenuService.GetAllRegions(new RecordsRange { PageSize = 500, PageNumber = 1 });

            Regions.Clear();
            foreach (var region in _regions.Items)
            {
                Regions.Add(region);
            }
        }

        public async void Add(object? obj)
        {
            int selectedRegionId = 0;
            var result = await _dialogService.ShowAsync<RegionsDialogViewModel, Region, int>(selectedRegionId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Edit(object? obj)
        {
            int selectedRegionId = SelectedRegion?.Id ?? 0;
            var result = await _dialogService.ShowAsync<RegionsDialogViewModel, Region, int>(selectedRegionId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedRegion?.Name}");

            if (result.IsOk())
            {
                int selectedRegionId = SelectedRegion?.Id ?? 0;
                try
                {
                    await _adminMenuService.DeleteRegion(selectedRegionId);
                }
                catch (OperationCannotBeExecutedException)
                {
                    MessageBox.Show("Nelze smazat region protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await InitializeAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedRegion?.Name != null;
        }
    }
}
