namespace Supermarket.Wpf.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Current view of application
        /// </summary>
        ApplicationView? CurrentView { get; }
        
        /// <summary>
        /// Raised when navigation to other view was successful
        /// </summary>
        event EventHandler<NavigationEventArgs> NavigationSucceeded;
        
        /// <summary>
        /// Tries to navigate to other view
        /// </summary>
        Task NavigateToAsync(ApplicationView applicationView);
        
        /// <summary>
        /// Tries to go back on previous view,
        /// nothing happen when there is no previous view
        /// </summary>
        Task BackAsync();
    }
}
