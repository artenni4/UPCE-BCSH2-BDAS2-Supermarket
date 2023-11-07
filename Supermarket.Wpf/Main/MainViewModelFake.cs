using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Menu;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Main;

public class MainViewModelFake : MainViewModel
{
    public MainViewModelFake() 
        : base(
            new NavigationServiceFake(),
            new ViewModelResolverFake(),
            new MenuViewModel(new NavigationServiceFake(), new LoggedUserServiceFake()))
    {
    }
}