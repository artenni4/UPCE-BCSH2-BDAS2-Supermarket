using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Manager.SupermarketProducts
{
    public class SupermarketProductsViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
