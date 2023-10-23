using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.CashBoxes;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.SoldProducts;
using Supermarket.Infrastructure.Common;
using SoldProduct = Supermarket.Domain.SoldProducts.SoldProduct;


namespace Supermarket.Infrastructure.SoldProducts
{
    internal class SoldProductRepository : CrudRepositoryBase<SoldProduct, int>, ISoldProductRepository
    {
        public SoldProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<SoldProduct>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
    }
}
