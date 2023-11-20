using System.Collections.ObjectModel;
using Supermarket.Core.Domain.ChangeLogs;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Admin.SupermarketLogs
{
    public class SupermarketLogsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncActivated
    {
        private readonly IAdminMenuService _adminMenuService;

        public SupermarketLogsViewModel(IAdminMenuService adminMenuService)
        {
            _adminMenuService = adminMenuService;
        }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Log";

        public ObservableCollection<ChangeLog> ChangeLogs { get; } = new();

        public async Task ActivateAsync()
        {
            using var _ = new DelegateLoading(this);
            var changeLogs = await _adminMenuService.GetChangeLogs(new RecordsRange { PageNumber = 1, PageSize = 10000 });
            ChangeLogs.Update(changeLogs.Items);
        }
    }
}
