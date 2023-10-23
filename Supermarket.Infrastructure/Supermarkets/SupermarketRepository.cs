using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Supermarkets;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Supermarkets;

internal class SupermarketRepository : CrudRepositoryBase<Domain.Supermarkets.Supermarket, int>, ISupermarketRepository
{
    public SupermarketRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public Task<PagedResult<Domain.Supermarkets.Supermarket>> GetPagedAsync(PagingQueryObject queryObject) =>
        GetRecordsRangeAsync(queryObject.RecordsRange);
}