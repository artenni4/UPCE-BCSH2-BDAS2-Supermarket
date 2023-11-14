using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
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
        private readonly ILoggedUserService _loggedUserService;

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public StoragePlace? StoragePlace { get; set; }
        public int StoragePlaceId { get; set; }
        public ObservableCollection<StoragePlaceTypeModel> StoragePlaceTypes { get; } = new ObservableCollection<StoragePlaceTypeModel>();
        public StoragePlaceTypeModel? SelectedStoragePlaceType { get; set; }

        public event EventHandler<DialogResult<StoragePlace>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SupermarketStoragesDialogViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (StoragePlaceId != 0)
            {
                StoragePlace = await _managerMenuService.GetStorageToEdit(StoragePlaceId);
            }
            else
            {
                StoragePlace = CreateNewStoragePlace();
            }

            if (StoragePlace != null)
            {
                SelectedStoragePlaceType = new StoragePlaceTypeModel(StoragePlace.Type);
            }

            GetPlaceTypes();
        }

        public async void SetParameters(int parameters)
        {
            StoragePlaceId = parameters;
            await InitializeAsync();
        }

        private StoragePlace CreateNewStoragePlace()
        {
            return new StoragePlace
            {
                Id = 0,
                Code = "",
                Location = null,
                Type = StoragePlaceType.SKLAD,
                SupermarketId = _loggedUserService.SupermarketId
            };
        }

        private void GetPlaceTypes()
        {
            var types = Enum.GetValues(typeof(StoragePlaceType)).Cast<StoragePlaceType>();
            StoragePlaceTypes.Clear();
            foreach (var type in types)
            {
                StoragePlaceTypes.Add(new StoragePlaceTypeModel(type));
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (StoragePlace != null && SelectedStoragePlaceType != null)
            {
                StoragePlace.Type = SelectedStoragePlaceType.Type;
                if (StoragePlaceId != 0)
                {
                    await _managerMenuService.EditStorage(StoragePlace);
                    ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Ok(StoragePlace));
                }
                else
                {
                    await _managerMenuService.AddStorage(StoragePlace);
                    ResultReceived?.Invoke(this, DialogResult<StoragePlace>.Ok(StoragePlace));
                }
                
            }

            //if (StoragePlace != null && StoragePlaceId == 0)
            //{
                
            //}
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

        


        
    }


#nullable enable

    public class StoragePlaceTypeModel : IEquatable<StoragePlaceTypeModel?>
    {
        public StoragePlaceType Type { get; set; }
        public string Name { get; set; }

        public StoragePlaceTypeModel(StoragePlaceType type)
        {
            Type = type;
            Name = type.ToString().ToUpper();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as StoragePlaceTypeModel);
        }

        public bool Equals(StoragePlaceTypeModel? other)
        {
            return other != null && Type == other.Type && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Name);
        }
    }



}

