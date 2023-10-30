using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.Session;

namespace Supermarket.Wpf;

public static class WpfDependencies
{
    public static IServiceCollection AddWpf(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<ILoggedUserService, SessionService>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection.AddTransient<LoginViewModel>();
        serviceCollection.AddTransient<CashboxViewModel>();

        return serviceCollection;
    }
}