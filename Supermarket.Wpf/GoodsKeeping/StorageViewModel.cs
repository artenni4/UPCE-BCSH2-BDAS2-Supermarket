using Supermarket.Core.GoodsKeeping;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement;
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

        private GoodsManagementViewModel? _goodsManagementViewModel;
        public GoodsManagementViewModel? GoodsManagementViewModel
        {
            get => _goodsManagementViewModel;
            set => SetProperty(ref _goodsManagementViewModel, value);
        }

        public StorageViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            ArrivalViewModel = await _viewModelResolver.Resolve<ArrivalRegistrationViewModel>();
            GoodsManagementViewModel = await _viewModelResolver.Resolve<GoodsManagementViewModel>();
        }
    }
}
