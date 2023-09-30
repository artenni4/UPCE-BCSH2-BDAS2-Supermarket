using Supermarket.Core.Common.Paging;
using Supermarket.Core.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products
{
    public class ProductRepository : CrudRepositoryBase<Product, int, ProductQueryObject>, IProductRepository
    {
        public override Task<PagedResult<Product>> GetPagedAsync(ProductQueryObject queryObject)
        {
            throw new NotImplementedException();
        }
    }
}
