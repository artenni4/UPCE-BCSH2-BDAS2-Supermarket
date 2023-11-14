namespace Supermarket.Wpf.Common;

/// <summary>
/// Describes object that can be activated when it is shown to user
/// </summary>
public interface IAsyncActivated
{
    /// <summary>
    /// Method that will be executed when user activated this object
    /// </summary>
    Task ActivateAsync();
}