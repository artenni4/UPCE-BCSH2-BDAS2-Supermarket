using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.ViewModelResolvers;

public class ViewModelResolverFake : IViewModelResolver
{
    public event EventHandler? InitializationStarted;
    public event EventHandler? InitializationFinished;
    public event EventHandler<ResolvedViewModelEventArgs>? ViewModelResolved;

    public Task<IViewModel> Resolve(Type viewModelType)
    {
        throw new NotImplementedException();
    }
}