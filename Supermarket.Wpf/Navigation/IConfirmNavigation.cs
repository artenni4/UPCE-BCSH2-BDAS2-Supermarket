using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Navigation
{
    internal interface IConfirmNavigation : IViewModel
    {
        /// <summary>
        /// Checks whether navigation out of the screen is permitted in current state
        /// </summary>
        bool CanNavigateFrom();

        /// <summary>
        /// Called when navigation from the screen was cancelled
        /// </summary>
        void NavigationCancelled();
    }
}
