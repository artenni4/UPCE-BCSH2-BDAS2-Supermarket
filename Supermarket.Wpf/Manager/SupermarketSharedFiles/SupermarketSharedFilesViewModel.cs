using Microsoft.Win32;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.Domain.SharedFiles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Manager.SupermarketCashboxes.Dialog;
using Supermarket.Wpf.Manager.SupermarketSharedFiles.Dialog;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketSharedFiles
{
    public class SupermarketSharedFilesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IDialogService _dialogService;

        private PagedResult<ManagerMenuSharedFile>? _files;
        public ObservableCollection<ManagerMenuSharedFile> Files { get; set; }

        private ManagerMenuSharedFile? _selectedFile;
        public ManagerMenuSharedFile? SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(Search));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand DownloadCommand { get; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Files";

        public SupermarketSharedFilesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService, IDialogService dialogService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;
            _dialogService = dialogService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanCallDialog);
            DeleteCommand = new RelayCommand(Delete, CanCallDialog);
            SearchCommand = new RelayCommand(Search);
            DownloadCommand = new RelayCommand(DownloadFile, CanCallDialog);

            Files = new();
        }

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);

            await GetFiles();
        }

        private async Task GetFiles()
        {
            _files = await _managerMenuService.GetSharedFiles(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 300, PageNumber = 1 }, SearchText);

            Files.Update(_files.Items);
        }

        public async void Search(object? obj)
        {
            await ActivateAsync();
        }

        private async void DownloadFile(object? obj)
        {
            if (SelectedFile == null)
            {
                return;
            }

            byte[] fileBytes;
            using (var _ = new DelegateLoading(this))
            {
                fileBytes = await _managerMenuService.DownloadSharedFile(SelectedFile.Id);
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = SelectedFile.FullName;

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;

                try
                {
                    await File.WriteAllBytesAsync(filePath, fileBytes);
                    MessageBox.Show("Soubor byl stahnout", "Probíhá uložení...", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba během stážení: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void Add(object? obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf|Word documents (*.docx)|*.docx|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                if (!_loggedUserService.HasEmployeeId(out var employeeId))
                {
                    throw new UiInconsistencyException("Unauthorized user is not allowed on this view");
                }

                var fileBytes = await File.ReadAllBytesAsync(selectedFilePath);

                var newSharedFile = new SharedFile
                {
                    Id = 0,
                    CreatedDate = DateTime.MinValue,
                    Name = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName),
                    Extenstion = Path.GetExtension(selectedFilePath),
                    EmployeeId = employeeId.Value,
                    ModifiedDate = DateTime.Now,
                    SupermarketId = _loggedUserService.SupermarketId
                };

                using var _ = new DelegateLoading(this);
                await _managerMenuService.AddSharedFile(newSharedFile, fileBytes);
            }
            await ActivateAsync();
        }

        private async void Edit(object? obj)
        {
            int selectedFileId = SelectedFile?.Id ?? 0;
            var result = await _dialogService.ShowAsync<SharedFilesDialogViewModel, SharedFile, int>(selectedFileId);
            if (result.IsOk(out var _))
            {
                await ActivateAsync();
            }
        }

        private async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedFile?.Name}");

            if (result.IsOk())
            {
                int selectedFileId = SelectedFile?.Id ?? 0;
                await _managerMenuService.DeleteSharedFile(selectedFileId);
                await ActivateAsync();
            }
        }

        private bool CanCallDialog(object? obj)
        {
            return SelectedFile != null;
        }

    }
}
