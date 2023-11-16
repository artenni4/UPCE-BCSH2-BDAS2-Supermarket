using Supermarket.Core.Domain.Regions;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SupermarketModel = Supermarket.Core.Domain.Supermarkets.Supermarket;

namespace Supermarket.Wpf.Admin.Supermarkets.Dialog
{
    class SupermarketsDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<SupermarketModel, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public SupermarketModel? Supermarket { get; set; }
        public int SupermarketId { get; set; }
        public ObservableCollection<Region> Regions { get; } = new ObservableCollection<Region>();
        public Region? SelectedRegion { get; set; }

        public event EventHandler<DialogResult<SupermarketModel>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SupermarketsDialogViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (SupermarketId != 0)
            {
                Supermarket = await _adminMenuService.GetSupermarket(SupermarketId);
            }
            else
            {
                Supermarket = new SupermarketModel
                {
                    Address = "",
                    Id = 0,
                    RegionId = 0
                };
            }

            if (Supermarket != null && SupermarketId != 0)
            {
                SelectedRegion = await _adminMenuService.GetRegion(Supermarket.RegionId);
            }

            GetRegions();
        }

        public async void SetParameters(int parameters)
        {
            SupermarketId = parameters;
            await InitializeAsync();
        }

        private async void GetRegions()
        {
            var regions = await _adminMenuService.GetAllRegions(new RecordsRange { PageSize = 250, PageNumber= 1 });
            Regions.Clear();
            foreach (var region in regions.Items)
            {
                Regions.Add(region);
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<SupermarketModel>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Supermarket != null && SelectedRegion != null)
            {
                var saveSupermarket = new SupermarketModel { Id = Supermarket.Id, Address = Supermarket.Address, RegionId = SelectedRegion.Id };
                if (SupermarketId != 0)
                {
                    await _adminMenuService.EditSupermarket(saveSupermarket);
                    ResultReceived?.Invoke(this, DialogResult<SupermarketModel>.Ok(saveSupermarket));
                }
                else
                {
                    await _adminMenuService.AddSupermarket(saveSupermarket);
                    ResultReceived?.Invoke(this, DialogResult<SupermarketModel>.Ok(saveSupermarket));
                }
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (!string.IsNullOrEmpty(Supermarket?.Address) && SelectedRegion != null)
            {
                return true;
            }
            else
                return false;
        }
    }
}
