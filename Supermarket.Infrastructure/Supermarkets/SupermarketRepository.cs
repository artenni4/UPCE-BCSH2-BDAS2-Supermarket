using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Infrastructure.StoragePlaces;

namespace Supermarket.Infrastructure.Supermarkets;

internal class SupermarketRepository : CrudRepositoryBase<Core.Domain.Supermarkets.Supermarket, int, DbSupermarket>, ISupermarketRepository
{
    public SupermarketRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<StoragePlace>> GetSupermarketWarehouses(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters().AddParameter("supermarket_id", supermarketId);
        const string sql = @"SELECT mu.*
                            FROM MISTA_ULOZENI mu
                            JOIN SUPERMARKETY s ON mu.supermarket_id = s.supermarket_id
                            WHERE s.supermarket_id = :supermarket_id AND mu.misto_ulozeni_typ = 'SKLAD'";
        var orderByColumns = DbStoragePlace.IdentityColumns.Select(ic => $"mu.{ic}");
        var result = await GetPagedResult<DbStoragePlace>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbStoragePlace => dbStoragePlace.ToDomainEntity());
    }

    public async Task<PagedResult<AdminSupermarket>> GetAdminSupermarkets(RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters();
        const string sql = @"SELECT
                                s.supermarket_id,
                                s.adresa,
                                s.region_id,
                                r.nazev as region_nazev
                            FROM
                                SUPERMARKETY s
                            JOIN
                                REGIONY r on r.region_id = s.region_id";

        var orderByColumns = DbAdminSupermarket.IdentityColumns.Select(ic => $"s.{ic}");
        var result = await GetPagedResult<DbAdminSupermarket>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbSupermarket => dbSupermarket.ToDomainEntity());
    }
}