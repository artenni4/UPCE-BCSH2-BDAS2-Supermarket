namespace Supermarket.Core.Products
{
    public interface IProductService
    {
        /// <summary>
        /// Returns a page with accessible products for a given supermarket
        /// </summary>
        Task<PagedResult<Product>> GetCashBoxProductsPage(int supermarketId, int productCategoryId, RecordsRange recordsRange);
    }
}
