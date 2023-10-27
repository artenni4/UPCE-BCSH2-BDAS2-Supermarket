using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.ProductCategories;
using Supermarket.Domain.Products;

namespace Supermarket.Domain.SellingProducts
{
    public interface ISellingProductRepository : ICrudRepository<SellingProduct, SellingProductId>
    {
        Task<PagedResult<Product>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange, int categoryId, string? searchText);
        Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange);
    }
}
