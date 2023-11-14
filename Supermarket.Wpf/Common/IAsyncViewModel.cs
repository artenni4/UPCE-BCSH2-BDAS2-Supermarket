namespace Supermarket.Wpf.Common;

public interface IAsyncViewModel : IViewModel
{
    event EventHandler LoadingStarted;
    event EventHandler LoadingFinished;
}