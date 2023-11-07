using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
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

    public event EventHandler? InitializationStarted;
    public event EventHandler? InitializationFinished;

    public async Task<IViewModel> Resolve(Type viewModelType)
    {
        var viewModel = _serviceProvider.GetRequiredService(viewModelType) as IViewModel;
        if (viewModel is null)
        {
            throw new ArgumentException($"{nameof(viewModelType.Name)} is not market with {nameof(IViewModel)} interface");
        }

        if (viewModel is IAsyncInitialized asyncInitialized)
        {
            InitializationStarted?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine($"View model initialization STARTED in thread {Environment.CurrentManagedThreadId}");

            try
            {
                await asyncInitialized.InitializeAsync();
            }
            finally
            {
                InitializationFinished?.Invoke(this, EventArgs.Empty);
                Debug.WriteLine($"View model initialization FINISHED in thread {Environment.CurrentManagedThreadId}");
            }
        }

        return viewModel;
    }
}