using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.ViewModelResolvers;

public class ViewModelResolverFake : IViewModelResolver
{
    public event EventHandler? InitializationStarted;
    public event EventHandler? InitializationFinished;
    public Task<object> Resolve(Type viewModelType)
    {
        throw new NotImplementedException();
    }
}