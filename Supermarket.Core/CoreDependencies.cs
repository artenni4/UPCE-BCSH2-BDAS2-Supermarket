using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ApplicationMenu;
using Supermarket.Core.UseCases.CashBox;
using Supermarket.Core.UseCases.Common;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Core.UseCases.Login;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core;

public static class CoreDependencies
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthDomainService, AuthDomainService>();

        serviceCollection.AddScopedWithProxy<IApplicationMenuService, ApplicationMenuService>();
        serviceCollection.AddScopedWithProxy<ILoginService, LoginService>();
        serviceCollection.AddScopedWithProxy<ICashBoxService, CashBoxService>();
        serviceCollection.AddScopedWithProxy<IGoodsKeepingService, GoodsKeepingService>();
        serviceCollection.AddScopedWithProxy<IManagerMenuService, ManagerMenuService>();
        serviceCollection.AddScopedWithProxy<IAdminMenuService, AdminMenuService>();


        return serviceCollection;
    }
}