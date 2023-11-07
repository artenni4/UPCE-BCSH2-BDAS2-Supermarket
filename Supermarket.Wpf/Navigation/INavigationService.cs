using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Navigation
{
    public interface INavigationService
    {
        ApplicationView? CurrentView { get; }
        event EventHandler<NavigationEventArgs> NavigationSucceeded;
        void NavigateTo(ApplicationView applicationView);
    }
}
