using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products;
using Supermarket.Domain.SellingProducts;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.SellingProducts
{
    internal class SellingProductRepository : CrudRepositoryBase<SellingProduct, int>, ISellingProductRepository
    {
        public SellingProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<SellingProduct>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
