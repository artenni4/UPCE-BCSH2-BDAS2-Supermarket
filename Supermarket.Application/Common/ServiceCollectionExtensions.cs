using Microsoft.Extensions.DependencyInjection;

namespace Supermarket.Core.Common
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service to container but every call to service's method will create new scope, 
        /// instantiate object in the scope and execute same method in a newly created scope
        /// </summary>
        public static IServiceCollection AddScopedWithProxy<TInterface, TImplmentation>(this IServiceCollection services)
            where TInterface : class
            where TImplmentation : class, TInterface
        {
            services.AddScoped<TImplmentation>();
            services.AddSingleton(CreateScopeDecorator<TInterface, TImplmentation>.Create);

            return services;
        }

        public static IServiceCollection AddConfigurationSection<TOptions>(this IServiceCollection services) 
            where TOptions : class, IConfigurationSection
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(TOptions.SectionName)
                .ValidateDataAnnotations();

            return services;
        }
    }
}
