using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Regions;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Regions;

internal class RegionRepository : CrudRepositoryBase<Region, int, DbRegion>, IRegionRepository
{
    public RegionRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}