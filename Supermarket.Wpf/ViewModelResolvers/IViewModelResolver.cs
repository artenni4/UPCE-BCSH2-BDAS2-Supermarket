using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.ViewModelResolvers;

public interface IViewModelResolver
{
    event EventHandler InitializationStarted;
    event EventHandler InitializationFinished;
    Task<object> Resolve(ApplicationView applicationView);
}