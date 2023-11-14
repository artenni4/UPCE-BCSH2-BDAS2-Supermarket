using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketStorages.Dialog
{
    public class SupermarketStoragesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<StoragePlace, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public StoragePlace? StoragePlace { get; set; }
        public int StoragePlaceId { get; set; }
        public ObservableCollection<StoragePlaceTypeModel> StoragePlaceTypes { get; } = new ObservableCollection<StoragePlaceTypeModel>();

        public event EventHandler<DialogResult<StoragePlace>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SupermarketStoragesDialogViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (StoragePlace != null && StoragePlaceId != 0)
            {
                await _managerMenuService.EditStorage(StoragePlace);
                ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Ok(StoragePlace));
            }

            if (StoragePlace != null && StoragePlaceId == 0)
            {
                await _managerMenuService.AddStorage(StoragePlace);
                ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Ok(StoragePlace));
            }


        }

        private bool CanConfirmEdit(object? arg)
        {
            if (StoragePlace?.Code != string.Empty)
            {
                return true;
            }
            else 
                return false;
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            StoragePlace = await _managerMenuService.GetStorageToEdit(StoragePlaceId);

            var types = Enum.GetValues(typeof(StoragePlaceType)).Cast<StoragePlaceType>();
            StoragePlaceTypes.Clear();
            foreach (var type in types)
            {
                StoragePlaceTypes.Add(new StoragePlaceTypeModel(type));
            }

        }


        public async void SetParameters(int parameters)
        {
            StoragePlaceId = parameters;
            await InitializeAsync();
        }

        
    }


    public class StoragePlaceTypeModel
    {
        public StoragePlaceType Type { get; set; }
        public string Name { get; set; }

        public StoragePlaceTypeModel(StoragePlaceType type)
        {
            Type = type;
            Name = type.ToString().ToUpper();
        }
    }

}

