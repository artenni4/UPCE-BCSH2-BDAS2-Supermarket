using System.Collections.ObjectModel;
using Supermarket.Core.Domain.ChangeLogs;
using Supermarket.Core.Domain.UsedDatabaseObjects;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Admin.UsedDatabaseObjects
{
    public class UsedDatabaseObjectsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IAdminMenuService _adminMenuService;

        public UsedDatabaseObjectsViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;
        }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "DB Objekty";

        public ObservableCollection<UsedDatabaseObject> UsedDatabaseObjects { get; } = new();

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);
            var usedDatabaseObjects = await _adminMenuService.GetUsedDatabaseObjects(new RecordsRange { PageNumber = 1, PageSize = 1000 });
            UsedDatabaseObjects.Update(usedDatabaseObjects.Items);
        }
    }
}
