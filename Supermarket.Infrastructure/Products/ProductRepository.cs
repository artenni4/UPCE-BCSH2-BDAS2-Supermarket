using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products
{
    internal class ProductRepository : CrudRepositoryBase<Product, int>, IProductRepository
    {
        public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<Product>> GetPagedAsync(ProductQueryObject queryObject)
        {
            throw new NotImplementedException();
        }
    }
}
