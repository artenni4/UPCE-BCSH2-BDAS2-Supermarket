using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.UseCases.CashBoxes;
using Supermarket.Core.UseCases.Common;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Core.UseCases.Login;

namespace Supermarket.Core;

public static class CoreDependencies
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthDomainService, AuthDomainService>();
        
        serviceCollection.AddScopedWithProxy<ILoginService, LoginService>();
        serviceCollection.AddScopedWithProxy<ICashBoxService, CashBoxService>();
        serviceCollection.AddScopedWithProxy<IGoodsKeepingService, GoodsKeepingService>();

        return serviceCollection;
    }
}