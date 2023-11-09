using Supermarket.Wpf.Common;
using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement;
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

        private GoodsManagementViewModel _goodsManagementViewModel;
        public GoodsManagementViewModel GoodsManagementViewModel
        {
            get => _goodsManagementViewModel;
            set
            {
                SetProperty(ref _goodsManagementViewModel, value);
            }
        }

        public StorageViewModel(ArrivalRegistrationViewModel arrivalViewModel, GoodsManagementViewModel goodsManagementViewModel)
        {
            _arrivalViewModel = arrivalViewModel;
            _goodsManagementViewModel = goodsManagementViewModel;
        }

        public async Task InitializeAsync()
        {
            await ArrivalViewModel.InitializeAsync();
            await GoodsManagementViewModel.InitializeAsync();
        }
    }
}
