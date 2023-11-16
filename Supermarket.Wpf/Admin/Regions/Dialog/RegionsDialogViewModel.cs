using Supermarket.Core.Domain.Regions;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Input;

namespace Supermarket.Wpf.Admin.Regions.Dialog
{
    public class RegionsDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<Region, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public Region? Region { get; set; }
        public int RegionId { get; set; }

        public event EventHandler<DialogResult<Region>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public RegionsDialogViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (RegionId != 0)
            {
                Region = await _adminMenuService.GetRegion(RegionId);
            }
            else
            {
                Region = new Region
                {
                    Id = 0,
                    Name = ""
                };
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<Region>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (Region != null)
            {
                if (RegionId != 0)
                {
                    await _adminMenuService.EditRegion(Region);
                    ResultReceived?.Invoke(this, DialogResult<Region>.Ok(Region));
                }
                else
                {
                    await _adminMenuService.AddRegion(Region);
                    ResultReceived?.Invoke(this, DialogResult<Region>.Ok(Region));
                }
            }
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(Region?.Name))
            {
                return true;
            }
            else
                return false;
        }

        public async void SetParameters(int parameters)
        {
            RegionId = parameters;
            await InitializeAsync();
        }
    }
}
