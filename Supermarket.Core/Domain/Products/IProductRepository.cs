using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.Products
{
    public interface IProductRepository : ICrudRepository<Product, int>
    {
        Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<AdminProduct>> GetAdminProducts(RecordsRange recordsRange);
    }
}
