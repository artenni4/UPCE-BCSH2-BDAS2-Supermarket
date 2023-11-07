using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Login;

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

    public async Task<object> Resolve(ApplicationView applicationView)
    {
        object viewModel = applicationView switch
        {
            ApplicationView.Login => _serviceProvider.GetRequiredService<LoginViewModel>(),
            ApplicationView.CashBox => _serviceProvider.GetRequiredService<CashboxViewModel>(),
            _ => throw new NotImplementedException($"Navigation to {applicationView} is not supported yet, implement it by extending this swith")
        };

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