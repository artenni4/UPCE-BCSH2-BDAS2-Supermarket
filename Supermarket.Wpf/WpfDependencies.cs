using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.CashBox;
using Supermarket.Wpf.Common.Dialogs.Confirmation;
using Supermarket.Wpf.Common.Dialogs.DropDown;
using Supermarket.Wpf.Common.Dialogs.Input;
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
using Supermarket.Wpf.Manager.SupermarketCashboxes;
using Supermarket.Wpf.Manager.SupermarketEmployees;
using Supermarket.Wpf.Manager.SupermarketLogs;
using Supermarket.Wpf.Manager.SupermarketProducts;
using Supermarket.Wpf.Manager.SupermarketSales;
using Supermarket.Wpf.Manager.SupermarketStorages;
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
        serviceCollection.AddTransient<CashBoxViewModel>();

        serviceCollection.AddTransient<ConfirmationDialogViewModel>();
        serviceCollection.AddTransient<InputDialogViewModel>();
        serviceCollection.AddTransient<DropDownDialogViewModel>();
        serviceCollection.AddTransient<LoginAssistantViewModel>();

        serviceCollection.AddTransient<StorageViewModel>();
        serviceCollection.AddTransient<ArrivalRegistrationViewModel>();
        serviceCollection.AddTransient<GoodsManagementViewModel>();
        serviceCollection.AddTransient<MoveStoredProductViewModel>();

        serviceCollection.AddTransient<ManagerMenuViewModel>();
        serviceCollection.AddTransient<SupermarketProductsViewModel>();
        serviceCollection.AddTransient<AddProductsViewModel>();
        serviceCollection.AddTransient<SupermarketEmployeesViewModel>();
        serviceCollection.AddTransient<SupermarketStoragesViewModel>();
        serviceCollection.AddTransient<SupermarketLogsViewModel>();
        serviceCollection.AddTransient<SupermarketSalesViewModel>();
        serviceCollection.AddTransient<SupermarketCashboxesViewModel>();

        return serviceCollection;
    }
}