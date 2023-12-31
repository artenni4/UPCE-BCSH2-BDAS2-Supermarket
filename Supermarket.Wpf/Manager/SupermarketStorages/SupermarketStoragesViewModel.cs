﻿using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Manager.SupermarketStorages.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Supermarket.Core.Domain.Common;

namespace Supermarket.Wpf.Manager.SupermarketStorages
{
    public class SupermarketStoragesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Místa uložení";

        private PagedResult<StoragePlace>? _storagePlaces;
        public ObservableCollection<StoragePlace> StoragePlaces { get; set; }

        private StoragePlace? _selectedStorage;
        public StoragePlace? SelectedStorage
        {
            get => _selectedStorage;
            set
            {
                _selectedStorage = value;
                OnPropertyChanged();
            }
        }

        public SupermarketStoragesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService, IDialogService dialogService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(AddStorage);
            EditCommand = new RelayCommand(EditStorage, CanCallDialog);
            DeleteCommand = new RelayCommand(DeleteStorage, CanCallDialog);

            StoragePlaces = new();
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            await GetStorages();
        }

        private async Task GetStorages()
        {
            StoragePlaces.Clear();
            _storagePlaces = await _managerMenuService.GetStoragePlaces(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 300, PageNumber = 1 });
            foreach (var stPlace in _storagePlaces.Items)
            {
                StoragePlaces.Add(stPlace);
            }
        }

        public async void AddStorage(object? obj)
        {
            int selectedStorageId = 0;
            
            var result = await _dialogService.ShowAsync<SupermarketStoragesDialogViewModel, StoragePlace, int> (selectedStorageId);
            if (result.IsOk(out var a))
            {
                await ActivateAsync();
            }
        }

        public async void EditStorage(object? obj)
        {
            int selectedStorageId = SelectedStorage?.Id ?? 0;

            var result = await _dialogService.ShowAsync<SupermarketStoragesDialogViewModel, StoragePlace, int>(selectedStorageId);
            if (result.IsOk(out var a))
            {
                await ActivateAsync();
            }
        }

        public async void DeleteStorage(object? obj)
        {
            Debug.Assert(SelectedStorage != null, nameof(SelectedStorage) + " != null");
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedStorage.Code}");

            if (!result.IsOk())
            {
                return;
            }
            
            try
            {
                await _managerMenuService.DeleteStorage(SelectedStorage.Id);
            }
            catch (ConstraintViolatedException)
            {
                var resultDelete = await _dialogService.ShowAsync<SupermarketStoragesMoveDialogViewModel, StoragePlace, int>(SelectedStorage.Id);
                if (resultDelete.IsOk(out var a))
                {
                    await ActivateAsync();
                }
                //MessageBox.Show("Místo uložení nelze smazat protože již se používá", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await ActivateAsync();
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedStorage != null;
        }
    }
}
