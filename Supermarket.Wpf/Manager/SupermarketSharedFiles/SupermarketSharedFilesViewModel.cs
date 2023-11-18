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
    public class SupermarketSharedFilesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
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

        public async Task InitializeAsync()
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
            await InitializeAsync();
        }

        public void DownloadFile(object? obj)
        {
            if (SelectedFile != null)
            {
                byte[] fileBytes = SelectedFile.Data;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = SelectedFile.Name + SelectedFile.Extenstion;

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        File.WriteAllBytes(filePath, fileBytes);
                        MessageBox.Show("Soubor byl stahnout", "Probíhá uložení...", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba během stážení: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                int userId = 0;
                string userName = "";
                _loggedUserService.IsEmployee(out var emp);
                if (emp == null)
                {
                    _loggedUserService.IsAdmin(out var admin);
                    if (admin != null)
                    {
                        userId = admin.Id;
                        userName = admin.Name + " " + admin.Surname;
                    }
                }
                else
                {
                    userId = emp.Id;
                    userName = emp.Name + " " + emp.Surname;
                }

                byte[] fileBytes = File.ReadAllBytes(selectedFilePath);

                SharedFile newSharedFile = new SharedFile
                {
                    Id = 0,
                    CreatedDate = DateTime.MinValue,
                    Name = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName),
                    Extenstion = Path.GetExtension(selectedFilePath),
                    Data = fileBytes,
                    EmployeeId = userId,
                    EmployeeName = userName,
                    ModifiedDate = DateTime.Now,
                    SupermarketId = _loggedUserService.SupermarketId
                };
                await _managerMenuService.AddSharedFile(newSharedFile);
            }
            await InitializeAsync();
        }

        public async void Edit(object? obj)
        {
            int selectedFileId = SelectedFile?.Id ?? 0;
            var result = await _dialogService.ShowAsync<SharedFilesDialogViewModel, SharedFile, int>(selectedFileId);
            if (result.IsOk(out var _))
            {
                await InitializeAsync();
            }
        }

        public async void Delete(object? obj)
        {
            var result = await _dialogService.ShowConfirmationDialogAsync($"Provedením této akce odstraníte {SelectedFile?.Name}");

            if (result.IsOk())
            {
                int selectedFileId = SelectedFile?.Id ?? 0;
                await _managerMenuService.DeleteSharedFile(selectedFileId);
                await InitializeAsync();
            }
        }

        public bool CanCallDialog(object? obj)
        {
            return SelectedFile != null;
        }

    }
}
