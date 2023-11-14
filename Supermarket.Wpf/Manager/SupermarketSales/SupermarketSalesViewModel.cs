using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Wpf.Manager.SupermarketSales
{
    public class SupermarketSalesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel/*, IAsyncInitialized*/
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Prodeje";

        public SupermarketSalesViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
