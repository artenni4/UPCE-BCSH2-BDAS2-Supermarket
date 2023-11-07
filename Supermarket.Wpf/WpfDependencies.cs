using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Menu;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf;

public static class WpfDependencies
{
    public static IServiceCollection AddWpf(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<IViewModelResolver, ViewModelResolver>();
        serviceCollection.AddSingleton<ILoggedUserService, LoggedUserService>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<MenuViewModel>();

        serviceCollection.AddTransient<LoginViewModel>();
        serviceCollection.AddTransient<CashboxViewModel>();

        return serviceCollection;
    }
}