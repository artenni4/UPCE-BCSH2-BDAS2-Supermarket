using Supermarket.Core.GoodsKeeping;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoredProducts;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.GoodsKeeping.GoodsManagement
{
    public class GoodsManagementViewModel : NotifyPropertyChangedBase, IAsyncViewModel, IAsyncInitialized
    {
        private readonly IGoodsKeepingService _goodsKeepingService;

        private PagedResult<GoodsKeepingStoredProduct>? storedProducts;
        public ObservableCollection<GoodsKeepingStoredProduct> StoredProducts { get; set; }

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public GoodsManagementViewModel(IGoodsKeepingService goodsKeepingService)
        {
            _goodsKeepingService = goodsKeepingService;

            StoredProducts = new();
        }

        public async Task InitializeAsync()
        {
            using var _ = new DelegateLoading(this);

            storedProducts = await _goodsKeepingService.GetStoredProducts(1, new RecordsRange { PageSize = 25, PageNumber = 1 });
            foreach(var item in storedProducts.Items)
            {
                StoredProducts.Add(item);
            }
        }
    }
}
