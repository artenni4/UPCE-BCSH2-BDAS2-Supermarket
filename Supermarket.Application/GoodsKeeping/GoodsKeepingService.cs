using Supermarket.Core.CashBoxes;
using Supermarket.Domain.SellingProducts;
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
        private readonly ISellingProductRepository _sellingProductRepository;

        public GoodsKeepingService(IStoragePlaceRepository storagePlaceRepository, ISellingProductRepository sellingProductRepository)
        {
            _storagePlaceRepository = storagePlaceRepository;
            _sellingProductRepository = sellingProductRepository;
        }

        public Task DeleteProductStorageAsync(int storagePlaceId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<GoodsKeepingProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _sellingProductRepository.GetSupermarketProductCategories(supermarketId, recordsRange);

            return result.Select(GoodsKeepingProductCategory.FromProductCategory);
        }

        public async Task<PagedResult<GoodsKeepingProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
        {
            var result = await _sellingProductRepository.GetSupermarketProducts(supermarketId, recordsRange, productCategoryId, searchText);

            return result.Select(GoodsKeepingProduct.FromProduct);
        }

        public async Task<PagedResult<GoodsKeepingStoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);

            return result.Select(GoodsKeepingStoragePlace.FromStoragePlace);
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
