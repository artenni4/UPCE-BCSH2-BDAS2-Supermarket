using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;
using StoredProduct = Supermarket.Domain.StoredProducts.StoredProduct;

namespace Supermarket.Infrastructure.StoredProducts
{
    internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, int>, IStoredProductRepository
    {
        public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<StoredProduct>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
