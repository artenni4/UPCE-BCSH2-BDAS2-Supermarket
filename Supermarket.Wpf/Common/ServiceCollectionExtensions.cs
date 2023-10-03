using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Common
{
    internal static class ServiceCollectionExtensions
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
    }
}
