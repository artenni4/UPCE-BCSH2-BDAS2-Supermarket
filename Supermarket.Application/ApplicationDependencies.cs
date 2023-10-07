using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.Login;
using Supermarket.Core.SelfCheckout;

namespace Supermarket.Core;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScopedWithProxy<ILoginService, LoginService>();
        serviceCollection.AddScopedWithProxy<ISelfCheckoutService, SelfCheckoutService>();

        return serviceCollection;
    }
}