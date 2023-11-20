using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.ChangeLogs;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Infrastructure.ChangeLogs;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly OracleConnection _oracleConnection;

    public ChangeLogRepository(OracleConnection oracleConnection)
    {
        _oracleConnection = oracleConnection;
    }

    public async Task<PagedResult<ChangeLog>> GetChangeLogs(RecordsRange recordsRange)
    {
        var pagingParameters = recordsRange.GetPagingParameters();
        const string sql = "SELECT * FROM LOGY ORDER BY cas DESC OFFSET :PagingOffset ROWS FETCH NEXT :PagingRowsCount ROWS ONLY";
        var changeLogs = await _oracleConnection.QueryAsync<DbChangeLog>(sql, pagingParameters);
        var items = changeLogs
            .Select(cl => cl.ToDomainEntity())
            .ToArray();
        
        return new PagedResult<ChangeLog>(items, recordsRange, items.Length);
    }
}