using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Infrastructure.CashBoxes;

internal class CashBoxRepository : CrudRepositoryBase<CashBox, int, DbCashBox>, ICashBoxRepository
{
    public CashBoxRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<CashBox>> GetBySupermarketId(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        var sql = $"SELECT * FROM {DbCashBox.TableName} WHERE supermarket_id = :supermarket_id";
        
        var pagedItems =
            await GetPagedResult<DbCashBox>(recordsRange, sql, DbCashBox.IdentityColumns, parameters);
            
        return pagedItems.Select(i => i.ToDomainEntity());
    }
}