using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.ViewModelResolvers;

public interface IAsyncInitialized
{
    Task InitializeAsync();
}