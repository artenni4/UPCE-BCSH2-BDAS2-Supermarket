using Supermarket.Wpf.Common;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.ViewModelResolvers;
using System.Threading.Tasks;

namespace Supermarket.Wpf.GoodsKeeping
{
    public class StorageViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IViewModelResolver _viewModelResolver;
        
        private ArrivalRegistrationViewModel? _arrivalViewModel;
        public ArrivalRegistrationViewModel? ArrivalViewModel
        {
            get => _arrivalViewModel;
            set => SetProperty(ref _arrivalViewModel, value);
        }

        public StorageViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            ArrivalViewModel = await _viewModelResolver.Resolve<ArrivalRegistrationViewModel>();
        }
    }
}
