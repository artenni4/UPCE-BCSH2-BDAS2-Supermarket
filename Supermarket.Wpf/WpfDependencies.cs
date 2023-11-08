using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.GoodsKeeping;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Navigation;
using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf;

public static class WpfDependencies
{
    public static IServiceCollection AddWpf(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddSingleton<IViewModelResolver, ViewModelResolver>();
        serviceCollection.AddSingleton<ILoggedUserService, LoggedUserService>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection.AddTransient<MenuViewModel>();
        serviceCollection.AddTransient<LoginViewModel>();
        serviceCollection.AddTransient<CashboxViewModel>();
        serviceCollection.AddTransient<StorageViewModel>();
        serviceCollection.AddTransient<ArrivalRegistrationViewModel>();
        serviceCollection.AddTransient<GoodsManagementViewModel>();

        serviceCollection.AddTransient<ProductCountInputViewModel>();
        serviceCollection.AddTransient<LoginAssistantViewModel>();

        return serviceCollection;
    }
}