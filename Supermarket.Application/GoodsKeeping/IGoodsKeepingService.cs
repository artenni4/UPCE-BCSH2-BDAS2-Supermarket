using Supermarket.Domain.StoredProducts;

namespace Supermarket.Core.GoodsKeeping;

public interface IGoodsKeepingService
{
    /// <summary>
    /// Returns a page with accessible products for a given supermarket
    /// </summary>
    Task<PagedResult<GoodsKeepingProduct>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText);

    /// <summary>
    /// Returns categories for display bar in cash box
    /// </summary>
    Task<PagedResult<GoodsKeepingProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange);

    /// <summary>
    /// Gets list of warehouses for goods supply
    /// </summary>
    Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange);
    
    /// <summary>
    /// Gets list of products that is stored in one of the storage place in the given supermarket
    /// </summary>
    Task<PagedResult<GoodsKeepingStoredProduct>> GetStoredProducts(int supermarketId, RecordsRange recordsRange);
    
    /// <summary>
    /// Adds products to specified warehouse in supermarket
    /// </summary>
    Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts);

    /// <summary>
    /// Moves product from one storage place to another
    /// </summary>
    Task MoveProductAsync(int storagePlaceId, MovingProduct movingProduct);

    /// <summary>
    /// Gets list of all storage places 
    /// </summary>
    Task<PagedResult<GoodsKeepingStoragePlace>> GetStoragePlacesAsync(int supermarketId, RecordsRange recordsRange);
    
    /// <summary>
    /// Deletes product from storage place
    /// </summary>
    Task DeleteProductStorageAsync(int storagePlaceId, int productId, decimal count);
}