using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs
{
    public class MoveStoredProductViewModel : NotifyPropertyChangedBase, IDialogViewModel<MoveProduct>, IAsyncViewModel, IAsyncInitialized
    {
        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<MoveProduct>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler?LoadingFinished;

        private readonly IGoodsKeepingService _goodsKeepingService;

        private PagedResult<SupplyWarehouse>? storagePlaces;
        public ObservableCollection<SupplyWarehouse> StoragePlaces { get; set; }

        private string? _productCount;
        public string? ProductCount
        {
            get => _productCount;
            set => SetProperty(ref _productCount, value);
        }

        private SupplyWarehouse? _selectedPlace;
        public SupplyWarehouse? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                OnPropertyChanged();
            }
        }

        public MoveStoredProductViewModel(IGoodsKeepingService goodsKeepingService)
        {
            _goodsKeepingService = goodsKeepingService;

            StoragePlaces = new();

            Confirm = new RelayCommand(ConfirmMove, CanConfirmMove);
            Cancel = new RelayCommand(CancelMove);
        }

        private void CancelMove(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<MoveProduct>.Cancel());
        }

        private void ConfirmMove(object? obj)
        {
            if (ProductCount is null)
                return;

            if (SelectedPlace is null)
                return;

            var result = new MoveProduct { Count = decimal.Parse(ProductCount), StorageId = SelectedPlace.Id } ;
            ResultReceived?.Invoke(this, DialogResult<MoveProduct>.Ok(result));
            
        }

        private bool CanConfirmMove(object? arg)
        {
            return decimal.TryParse(ProductCount, out _);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            storagePlaces = await _goodsKeepingService.GetWarehousesAsync(1, new RecordsRange { PageSize = 30, PageNumber = 1 });

            for (int i = 0; i < storagePlaces.Items.Count; i++)
            {
                StoragePlaces.Add(storagePlaces.Items[i]);
            }
        }


    }
}
