using Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;
using Supermarket.Wpf.GoodsKeeping.GoodsManagement;
using Supermarket.Wpf.Manager.SupermarketProducts;
using Supermarket.Wpf.ViewModelResolvers;
using System.Collections.ObjectModel;

namespace Supermarket.Wpf.GoodsKeeping
{
    public class StorageViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IViewModelResolver _viewModelResolver;
        public ObservableCollection<ITabViewModel> TabViewModels { get; } = new();

        private ITabViewModel? _selectedTabViewModel;
        public ITabViewModel? SelectedTabViewModel
        {
            get => _selectedTabViewModel;
            set
            {
                if (_selectedTabViewModel != value)
                {
                    SetProperty(ref _selectedTabViewModel, value);
                    _selectedTabViewModel?.ActivateIfNeeded();
                }
            }
        }

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
            var arrivalViewModel = await _viewModelResolver.Resolve<ArrivalRegistrationViewModel>();
            var goodsManagementViewModel = await _viewModelResolver.Resolve<GoodsManagementViewModel>();

            TabViewModels.Add(arrivalViewModel);
            TabViewModels.Add(goodsManagementViewModel);

            SelectedTabViewModel = arrivalViewModel;
        }
    }
}
