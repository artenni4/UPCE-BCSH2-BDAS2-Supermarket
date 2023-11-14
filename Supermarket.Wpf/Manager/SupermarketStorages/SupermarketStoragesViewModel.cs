using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Wpf.Manager.SupermarketStorages
{
    public class SupermarketStoragesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel/*, IAsyncInitialized*/
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Místa uložení";

        public SupermarketStoragesViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
