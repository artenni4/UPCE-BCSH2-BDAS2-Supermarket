using Supermarket.Domain.Products;
using Supermarket.Domain.Products.Categories;

namespace Supermarket.Core.GoodsKeeping;

public interface IGoodsKeepingService
{
    /// <summary>
    /// Returns a page with accessible products for a given supermarket
    /// </summary>
    Task<PagedResult<Product>> GetProductsAsync(int supermarketId, RecordsRange recordsRange, int productCategoryId, string? searchText);

    /// <summary>
    /// Returns categories for display bar in cash box
    /// </summary>
    Task<PagedResult<ProductCategory>> GetCategoriesAsync(int supermarketId, RecordsRange recordsRange);

    /// <summary>
    /// Gets list of warehouses for goods supply
    /// </summary>
    Task<PagedResult<SupplyWarehouse>> GetWarehousesAsync(int supermarketId, RecordsRange recordsRange);
    
    /// <summary>
    /// Adds products to specified warehouse in supermarket
    /// </summary>
    Task SupplyProductsToWarehouseAsync(int warehouseId, IReadOnlyList<SuppliedProduct> suppliedProducts);
    
    //TODO Task MoveProductAsync(...)
    //TODO Task DeleteProductStorageAsync(...)
}