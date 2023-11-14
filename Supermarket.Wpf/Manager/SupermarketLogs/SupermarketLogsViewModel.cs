using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Wpf.Manager.SupermarketLogs
{
    public class SupermarketLogsViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel/*, IAsyncInitialized*/
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Log";

        public SupermarketLogsViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
