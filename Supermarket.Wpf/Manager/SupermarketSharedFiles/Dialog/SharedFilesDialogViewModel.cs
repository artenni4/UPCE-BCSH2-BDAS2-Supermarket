using Supermarket.Core.Domain.SharedFiles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager.SupermarketSharedFiles.Dialog
{
    public class SharedFilesDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<SharedFile, int>, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        public SharedFile? SharedFile { get; set; }
        public int SharedFileId { get; set; }

        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        private int UserId { get; set; } = 0;
        private string UserName { get; set; } = string.Empty;

        public event EventHandler<DialogResult<SharedFile>>? ResultReceived;
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SharedFilesDialogViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Confirm = new RelayCommand(ConfirmEdit, CanConfirmEdit);
            Cancel = new RelayCommand(CancelEdit);
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            if (SharedFileId != 0)
            {
                SharedFile = await _managerMenuService.GetSharedFile(SharedFileId);
            }
            else
            {
                SharedFile = new SharedFile
                {
                    Id = 0,
                    Name = "",
                    CreatedDate = DateTime.Now,
                    EmployeeId = UserId,
                    Extenstion = "",
                    ModifiedDate = DateTime.Now,
                    SupermarketId = _loggedUserService.SupermarketId
                };
            }
        }

        private void CancelEdit(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<SharedFile>.Cancel());
        }

        private async void ConfirmEdit(object? obj)
        {
            if (SharedFile == null || SharedFileId != 0)
            {
                return;
            }
            
            var fileToSave = SharedFile = new SharedFile
            {
                Id = SharedFile.Id,
                Name = SharedFile.Name,
                CreatedDate = SharedFile.CreatedDate,
                EmployeeId = SharedFile.EmployeeId,
                Extenstion = SharedFile.Extenstion,
                ModifiedDate = DateTime.Now,
                SupermarketId = SharedFile.SupermarketId
            };
            await _managerMenuService.EditSharedFile(fileToSave);

            ResultReceived?.Invoke(this, DialogResult<SharedFile>.Ok(SharedFile));
        }

        private bool CanConfirmEdit(object? arg)
        {
            if (ValidateInput.IsValidStringInput(SharedFile?.Name) && ValidateInput.IsValidStringInput(SharedFile?.Name))
                return true;

            return false;
        }

        public async void SetParameters(int parameters)
        {
            SharedFileId = parameters;

            _loggedUserService.IsEmployee(out var emp);
            if (emp == null)
            {
                _loggedUserService.IsAdmin(out var admin);
                if (admin != null)
                {
                    UserId = admin.Id;
                    UserName = admin.Name + " " + admin.Surname;
                }
            }
            else
            {
                UserId = emp.Id;
                UserName = emp.Name + " " + emp.Surname;
            }

            await InitializeAsync();
        }


    }
}
