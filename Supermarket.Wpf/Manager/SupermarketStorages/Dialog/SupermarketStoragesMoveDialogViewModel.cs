using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketStorages.Dialog
{
    public class SupermarketStoragesMoveDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<StoragePlace, int>, IAsyncViewModel, IAsyncInitialized
    {
        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public async void SetParameters(int parameters)
        {
            Place = await _goodsKeepingService.GetStoragePlaceAsync(parameters);
            await InitializeAsync();
        }

        public event EventHandler<DialogResult<StoragePlace>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        private readonly IGoodsKeepingService _goodsKeepingService;
        private readonly ILoggedUserService _loggedUserService;

        public ObservableCollection<StoragePlace> StoragePlaces { get; set; }

        private string? _productCount;
        public string? ProductCount
        {
            get => _productCount;
            set => SetProperty(ref _productCount, value);
        }

        private StoragePlace? _place;
        public StoragePlace? Place
        {
            get => _place;
            set
            {
                SetProperty(ref _place, value);
                OnPropertyChanged();
            }
        }
        private StoragePlace? _selectedPlace;
        public StoragePlace? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                OnPropertyChanged();
            }
        }

        public SupermarketStoragesMoveDialogViewModel(IGoodsKeepingService goodsKeepingService, ILoggedUserService loggedUserService)
        {
            _goodsKeepingService = goodsKeepingService;
            _loggedUserService = loggedUserService;

            StoragePlaces = new();

            Confirm = new RelayCommand(ConfirmMove, CanConfirmMove);
            Cancel = new RelayCommand(CancelMove);
        }

        private void CancelMove(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Cancel());
        }

        private async void ConfirmMove(object? obj)
        {

            if (Place is null)
                return;

            if (SelectedPlace is null)
                return;

            await _goodsKeepingService.MoveProductsAndDelete(Place.Id, SelectedPlace.Id);

            var result = new StoragePlace { Code = SelectedPlace.Code, Id = SelectedPlace.Id, SupermarketId = SelectedPlace.SupermarketId, Type = SelectedPlace.Type };
            ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Ok(result));
        }

        private bool CanConfirmMove(object? arg)
        {
            if (SelectedPlace is null)
                return false;
            return true;
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            var storagePlaces = await _goodsKeepingService.GetStoragePlacesToMoveAsync(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });
            StoragePlaces.Clear();
            foreach (var place in storagePlaces.Items)
            {
                if (place.Id != Place?.Id)
                {
                    StoragePlaces.Add(place);
                }
            }
            
        }
    }
}
