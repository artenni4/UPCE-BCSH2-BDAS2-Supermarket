using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.SellingProducts
{
    public interface ISellingProductRepository : ICrudRepository<SellingProduct, SellingProductId>
    {
        Task<PagedResult<Product>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange, int categoryId, string? searchText);
        Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuProduct>> GetAllSupermarketProducts(int supermarketId, RecordsRange recordsRange);
    }
}
