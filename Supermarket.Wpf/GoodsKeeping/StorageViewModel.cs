using Supermarket.Wpf.Common;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.ViewModelResolvers;
using System.Threading.Tasks;

namespace Supermarket.Wpf.GoodsKeeping
{
    public class StorageViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private ArrivalRegistrationViewModel _arrivalViewModel;
        public ArrivalRegistrationViewModel ArrivalViewModel
        {
            get
            {
                return _arrivalViewModel;
            }

            set
            {
                SetProperty(ref _arrivalViewModel, value);
            }
        }

        public StorageViewModel(ArrivalRegistrationViewModel arrivalViewModel)
        {
            _arrivalViewModel = arrivalViewModel;
        }

        public async Task InitializeAsync()
        {
            await ArrivalViewModel.InitializeAsync();
        }
    }
}
