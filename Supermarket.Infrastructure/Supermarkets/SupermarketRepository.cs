using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Supermarkets;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Supermarkets;

internal class SupermarketRepository : CrudRepositoryBase<Domain.Supermarkets.Supermarket, int, SupermarketRepository.DbSupermarket, PagingQueryObject>, ISupermarketRepository
{
    public SupermarketRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
    
    public class DbSupermarket
    {
        public required int supermarket_id { get; init; }
    }

    protected override string TableName => "SUPERMARKETY";
    protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbSupermarket.supermarket_id) };
    protected override Domain.Supermarkets.Supermarket MapToEntity(DbSupermarket dbEntity)
    {
        throw new NotImplementedException();
    }

    protected override DbSupermarket MapToDbEntity(Domain.Supermarkets.Supermarket entity)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters GetIdentityValues(int id)
    {
        throw new NotImplementedException();
    }

    protected override int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}