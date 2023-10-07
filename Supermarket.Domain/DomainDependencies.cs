using Microsoft.Extensions.DependencyInjection;
using Supermarket.Domain.Auth;

namespace Supermarket.Domain;

public static class DomainDependencies
{
    public static IServiceCollection AddDomain(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthDomainService, AuthDomainService>();
        
        return serviceCollection;
    }
}