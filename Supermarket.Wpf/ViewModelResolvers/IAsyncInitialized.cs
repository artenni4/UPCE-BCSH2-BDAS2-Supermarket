using System.Threading.Tasks;

namespace Supermarket.Wpf.ViewModelResolvers;

public interface IAsyncInitialized
{
    Task InitializeAsync();
}