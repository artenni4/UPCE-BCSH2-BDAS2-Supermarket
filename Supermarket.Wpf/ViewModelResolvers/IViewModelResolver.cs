namespace Supermarket.Wpf.ViewModelResolvers;

public interface IViewModelResolver
{
    event EventHandler<ResolvedViewModelEventArgs> ViewModelResolved;
    Task<IViewModel> Resolve(Type viewModelType);
}