using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.UseCases.Common;

namespace Supermarket.Core.UseCases.GoodsKeeping
{
    public class GoodsKeepingService : IGoodsKeepingService
    {
        private readonly IStoragePlaceRepository _storagePlaceRepository;
        private readonly ISellingProductRepository _sellingProductRepository;
        private readonly ISupermarketRepository _supermarketRepository;
        private readonly IStoredProductRepository _storedProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GoodsKeepingService(IStoragePlaceRepository storagePlaceRepository,
            ISellingProductRepository sellingProductRepository,
            ISupermarketRepository supermarketRepository,
            IStoredProductRepository storedProductRepository,
            IUnitOfWork unitOfWork)
        {
            _storagePlaceRepository = storagePlaceRepository;
            _sellingProductRepository = sellingProductRepository;
            _supermarketRepository = supermarketRepository;
            _storedProductRepository = storedProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteProductStorageAsync(int storagePlaceId, int productId, decimal count)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _storedProductRepository.DeleteProductFormStorage(storagePlaceId, productId, count);
            await transaction.CommitAsync();
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

        public async Task<StoragePlace?> GetStoragePlaceAsync(int id)
        {
            return await _storagePlaceRepository.GetByIdAsync(id);
        }

        public async Task<PagedResult<GoodsKeepingStoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange)
        {
            var result = await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);

            return result.Select(GoodsKeepingStoragePlace.FromStoragePlace);
        }

        public async Task<PagedResult<StoragePlace>> GetStoragePlacesToMoveAsync(int supermarketId, RecordsRange recordsRange)
        {
            return await _storagePlaceRepository.GetSupermarketStoragePlaces(supermarketId, recordsRange);
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
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _storagePlaceRepository.MoveProduct(storagePlaceId, movingProduct);
            await transaction.CommitAsync();
        }

        public async Task MoveProductsAndDelete(int id, int newPlaceId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            await _storagePlaceRepository.MoveProductAndDelete(id, newPlaceId);
            await transaction.CommitAsync();
        }

        public async Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            var warehouse = await _storagePlaceRepository.GetByIdAsync(warehouseId) ?? throw new ApplicationInconsistencyException("Warehouse not found");
            foreach(var product in suppliedProducts)
            {
                await _storagePlaceRepository.SupplyProductsToWarehouse(warehouseId, product.ProductId, warehouse.SupermarketId, product.Count);
            }
            await transaction.CommitAsync();
        }
    }
}
