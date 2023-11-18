using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.Supermarkets;

namespace Supermarket.Core.UseCases.GoodsKeeping
{
    public class GoodsKeepingService : IGoodsKeepingService
    {
        private readonly IStoragePlaceRepository _storagePlaceRepository;
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly ISupermarketRepository _supermarketRepository;
        private readonly IStoredProductRepository _storedProductRepository;

        public GoodsKeepingService(IStoragePlaceRepository storagePlaceRepository, ISellingProductRepository sellingProductRepository, ISupermarketRepository supermarketRepository, IStoredProductRepository storedProductRepository)
        {
            _storagePlaceRepository = storagePlaceRepository;
            _sellingProductRepository = sellingProductRepository;
            _supermarketRepository = supermarketRepository;
            _storedProductRepository = storedProductRepository;
        }

        public async Task DeleteProductStorageAsync(int storagePlaceId, int productId, decimal count)
        {
            await _storedProductRepository.DeleteProductFormStorage(storagePlaceId, productId, count);
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

        public async Task<PagedResult<GoodsKeepingStoredProduct>> GetStoredProducts(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _storedProductRepository.GetSupermarketStoredProducts(supermarketId, recordsRange);

            return result;
        }

        public async Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _supermarketRepository.GetSupermarketWarehouses(supermarketId, recordsRange);

            return result.Select(SupplyWarehouse.FromStoragePlace);
        }

        public async Task MoveProductAsync(int storagePlaceId, MovingProduct movingProduct)
        {
            await _storagePlaceRepository.MoveProduct(storagePlaceId, movingProduct);
        }

        public async Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts, int supermarketId)
        {
            var warehouse = await _storagePlaceRepository.GetByIdAsync(warehouseId) ?? throw new Exception();
            foreach(var product in suppliedProducts)
            {
                await _storagePlaceRepository.SupplyProductsToWarehouse(warehouseId, product.ProductId, supermarketId, product.Count);
                //var id = new StoredProductId(warehouseId, warehouse.SupermarketId, product.ProductId);
                //var storedProduct = await _storedProductRepository.GetByIdAsync(id);
                //if (storedProduct != null)
                //{
                //    var newStoredProduct = new StoredProduct { Id = id, Count = storedProduct.Count + product.Count };
                //    await _storedProductRepository.UpdateAsync(newStoredProduct);
                //}
                //else
                //    await _storedProductRepository.AddAsync(new StoredProduct { Id = id, Count = product.Count });
            }
        }
    }
}
