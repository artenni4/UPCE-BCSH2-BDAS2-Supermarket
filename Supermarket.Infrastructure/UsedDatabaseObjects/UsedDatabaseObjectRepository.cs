using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.ChangeLogs;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.UsedDatabaseObjects;
using Supermarket.Infrastructure.ChangeLogs;

namespace Supermarket.Infrastructure.UsedDatabaseObjects;

public class UsedDatabaseObjectRepository : IUsedDatabaseObjectRepository
{
    private readonly OracleConnection _oracleConnection;

    public UsedDatabaseObjectRepository(OracleConnection oracleConnection)
    {
        _oracleConnection = oracleConnection;
    }

    public async Task<PagedResult<UsedDatabaseObject>> GetUsedDatabaseObjects(RecordsRange recordsRange)
    {
        var pagingParameters = recordsRange.GetPagingParameters();
        const string sql = "SELECT object_name, object_type FROM user_objects ORDER BY object_type OFFSET :PagingOffset ROWS FETCH NEXT :PagingRowsCount ROWS ONLY";
        var changeLogs = await _oracleConnection.QueryAsync<DbUsedDatabaseObjectRepository>(sql, pagingParameters);
        var items = changeLogs
            .Select(cl => cl.ToDomainEntity())
            .ToArray();
        
        return new PagedResult<UsedDatabaseObject>(items, recordsRange, items.Length);
    }
}