using Supermarket.Domain.StoragePlaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.GoodsKeeping
{
    public class GoodsKeepingService : IGoodsKeepingService
    {
        private readonly IStoragePlaceRepository _storagePlaceRepository;

        public GoodsKeepingService(IStoragePlaceRepository storagePlaceRepository)
        {
            _storagePlaceRepository = storagePlaceRepository;
        }

        public Task DeleteProductStorageAsync(int storagePlaceId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<GoodsKeepingProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<GoodsKeepingProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<StoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);

            throw new NotImplementedException();
        }

        public Task<PagedResult<StoredProduct>> GetStoredProducts(int supermarketId, RecordsRange recordsRange)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange)
        {
            throw new NotImplementedException();
        }

        public Task MoveProductAsync(int storagePlaceId, MovingProduct movingProduct)
        {
            throw new NotImplementedException();
        }

        public Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts)
        {
            throw new NotImplementedException();
        }
    }
}
