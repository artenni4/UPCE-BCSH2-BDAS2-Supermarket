using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Manager.SupermarketEmployees
{
    public class SupermarketEmployeesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IManagerMenuService _managerMenuService;
        private readonly ILoggedUserService _loggedUserService;

        private PagedResult<ManagerMenuEmployee>? _employees;
        public ObservableCollection<ManagerMenuEmployee> Employees { get; set; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Zaměstnanci";

        public SupermarketEmployeesViewModel(IManagerMenuService managerMenuService, ILoggedUserService loggedUserService)
        {
            _managerMenuService = managerMenuService;
            _loggedUserService = loggedUserService;

            Employees = new();
        }

        public async Task InitializeAsync()
        {
            _employees = await _managerMenuService.GetManagerEmployees(_loggedUserService.SupermarketId, new RecordsRange { PageSize = 250, PageNumber = 1 });
        }

    }
}
