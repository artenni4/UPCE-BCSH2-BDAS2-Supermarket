using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Supermarket.Core.UseCases.Common
{
    /// <summary>
    /// Decorator for methods that creates new scope before execution of method
    /// in which instantiates a service and executes same method within new scope.
    /// Use in couple with <see cref="ServiceCollectionExtensions.AddScopedWithProxy{TInterface, TImplmentation}(IServiceCollection)"/>
    /// </summary>
    /// <typeparam name="TService">Service implementation to be decorated</typeparam>
    /// <typeparam name="TInterface">Service interface</typeparam>
    internal class CreateScopeDecorator<TInterface, TService> : DispatchProxy
        where TInterface : class
        where TService : class, TInterface
    {
        private IServiceProvider? _serviceProvider;

        private void SetParameters(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            ArgumentNullException.ThrowIfNull(_serviceProvider, nameof(_serviceProvider));
            ArgumentNullException.ThrowIfNull(targetMethod, nameof(targetMethod));

            using var scope = _serviceProvider.CreateScope();
            var scopedObject = scope.ServiceProvider.GetRequiredService<TService>();
            
            return targetMethod.Invoke(scopedObject, args);
        }

        public static TInterface Create(IServiceProvider serviceProvider)
        {
            object proxy = Create<TInterface, CreateScopeDecorator<TInterface, TService>>();
            ((CreateScopeDecorator<TInterface, TService>)proxy).SetParameters(serviceProvider);

            return (TInterface)proxy;
        }
    }
}
