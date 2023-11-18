using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.UseCases.GoodsKeeping;

namespace Supermarket.Wpf.GoodsKeeping;

public class GoodsKeepingServiceFake : IGoodsKeepingService
{
    public Task<PagedResult<GoodsKeepingProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResult<GoodsKeepingProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResult<GoodsKeepingStoredProduct>> GetStoredProducts(int supermarketId, RecordsRange recordsRange)
    {
        throw new System.NotImplementedException();
    }

    public Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts)
    {
        throw new System.NotImplementedException();
    }

    public Task MoveProductAsync(int storagePlaceId, MovingProduct movingProduct)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResult<GoodsKeepingStoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteProductStorageAsync(int storagePlaceId, int productId, decimal count)
    {
        throw new System.NotImplementedException();
    }

    public Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts, int supermarketId)
    {
        throw new NotImplementedException();
    }

    public Task<StoragePlace?> GetStoragePlaceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task MoveProductsAndDelete(int id, int newPlaceId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<StoragePlace>> GetStoragePlacesToMoveAsync(int supermarketId, RecordsRange recordsRange)
    {
        throw new NotImplementedException();
    }
}