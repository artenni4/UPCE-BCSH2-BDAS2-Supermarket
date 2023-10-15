using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.CashBoxes;
using Supermarket.Core.Login;

namespace Supermarket.Core;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScopedWithProxy<ILoginService, LoginService>();
        serviceCollection.AddScopedWithProxy<ICashBoxService, CashBoxService>();

        return serviceCollection;
    }
}