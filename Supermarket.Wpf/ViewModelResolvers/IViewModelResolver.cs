using System;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.ViewModelResolvers;

public interface IViewModelResolver
{
    event EventHandler<ResolvedViewModelEventArgs> ViewModelResolved;
    Task<IViewModel> Resolve(Type viewModelType);
}