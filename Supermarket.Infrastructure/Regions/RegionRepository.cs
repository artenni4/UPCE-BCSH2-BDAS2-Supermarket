using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Regions;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Regions;

internal class RegionRepository : CrudRepositoryBase<Region, int>, IRegionRepository
{
    public RegionRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public Task<PagedResult<Region>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);
}