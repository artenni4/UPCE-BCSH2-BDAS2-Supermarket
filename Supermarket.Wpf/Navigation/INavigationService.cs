using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Navigation
{
    internal interface INavigationService
    {
        NavigateWindow? CurrentWindow { get; }
        event EventHandler<NavigationEventArgs> NavigationCompleted;
        void NavigateTo(NavigateWindow navigateWindow);
    }
}
