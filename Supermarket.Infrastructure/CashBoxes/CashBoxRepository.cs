using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.CashBoxes;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.UseCases.ManagerMenu;

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

    public async Task<PagedResult<ManagerMenuCashbox>> GetSupermarketCashboxes(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        var sql = $"SELECT * FROM POKLADNY WHERE supermarket_id = :supermarket_id";

        var pagedItems = await GetPagedResult<DbManagerMenuCashbox>(recordsRange, sql, DbManagerMenuCashbox.IdentityColumns, parameters);

        return pagedItems.Select(i => i.ToDomainEntity());
    }

    public async Task<ManagerMenuCashbox?> GetCashboxToEdit(int cashboxId)
    {
        var parameters = new DynamicParameters().AddParameter("pokladna_id", cashboxId);
        var sql = $"SELECT * FROM POKLADNY WHERE pokladna_id = :pokladna_id";

        var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbManagerMenuCashbox>(sql, parameters);

        return result?.ToDomainEntity();
    }

}