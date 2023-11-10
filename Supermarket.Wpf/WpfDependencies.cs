using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Common.Dialogs;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.GoodsKeeping;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Manager;
using Supermarket.Wpf.Manager.AddProducts;
using Supermarket.Wpf.Manager.SupermarketProducts;
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
        serviceCollection.AddSingleton<IManagerMenuService, ManagerMenuService>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection.AddTransient<MenuViewModel>();
        serviceCollection.AddTransient<LoginViewModel>();
        serviceCollection.AddTransient<CashboxViewModel>();

        serviceCollection.AddTransient<ConfirmationDialogViewModel>();
        serviceCollection.AddTransient<ProductCountInputViewModel>();
        serviceCollection.AddTransient<LoginAssistantViewModel>();

        serviceCollection.AddTransient<StorageViewModel>();
        serviceCollection.AddTransient<ArrivalRegistrationViewModel>();
        serviceCollection.AddTransient<GoodsManagementViewModel>();
        serviceCollection.AddTransient<DeleteStoredProductViewModel>();
        serviceCollection.AddTransient<MoveStoredProductViewModel>();

        serviceCollection.AddTransient<ManagerMenuViewModel>();
        serviceCollection.AddTransient<SupermarketProductsViewModel>();
        serviceCollection.AddTransient<AddProductsViewModel>();
        // ...

        return serviceCollection;
    }
}