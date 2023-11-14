using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Wpf.Manager.SupermarketCashboxes
{
    public class SupermarketCashboxesViewModel : NotifyPropertyChangedBase, ITabViewModel, IAsyncViewModel/*, IAsyncInitialized*/
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;
        public string TabHeader => "Pokladny";

        public SupermarketCashboxesViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
