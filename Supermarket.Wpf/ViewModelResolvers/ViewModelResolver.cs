using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.CashBox;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.ViewModelResolvers;

public class ViewModelResolver : IViewModelResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public event EventHandler<ResolvedViewModelEventArgs>? ViewModelResolved;

    public async Task<IViewModel> Resolve(Type viewModelType)
    {
        var viewModel = _serviceProvider.GetRequiredService(viewModelType) as IViewModel;
        if (viewModel is null)
        {
            throw new ArgumentException($"{nameof(viewModelType.Name)} is not market with {nameof(IViewModel)} interface");
        }
        
        ViewModelResolved?.Invoke(this, new ResolvedViewModelEventArgs { ViewModel = viewModel });
        if (viewModel is IAsyncInitialized asyncInitialized)
        {
            await asyncInitialized.InitializeAsync();
            Debug.WriteLine($"{viewModel} initialized in thread {Environment.CurrentManagedThreadId}");
        }

        return viewModel;
    }
}