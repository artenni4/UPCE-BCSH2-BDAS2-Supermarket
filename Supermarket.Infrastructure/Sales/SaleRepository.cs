using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Sales;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Sales
{
    internal class SaleRepository : CrudRepositoryBase<Sale, int>, ISaleRepository
    {
        public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<PagedResult<Sale>> GetPagedAsync(PagingQueryObject queryObject)
        {
            throw new NotImplementedException();
        }
    }
}
