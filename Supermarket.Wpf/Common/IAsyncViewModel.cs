namespace Supermarket.Wpf.Common;

/// <summary>
/// View model that requires async calls.
/// Calls should be wrapped into loading events.
/// </summary>
public interface IAsyncViewModel : IViewModel
{
    event EventHandler LoadingStarted;
    event EventHandler LoadingFinished;
}