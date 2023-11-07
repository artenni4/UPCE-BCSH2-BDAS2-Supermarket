using System;
using System.Threading.Tasks;

namespace Supermarket.Wpf.ViewModelResolvers;

public interface IViewModelResolver
{
    event EventHandler InitializationStarted;
    event EventHandler InitializationFinished;
    Task<object> Resolve(Type viewModelType);
}