using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Sales;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Sales;

internal class SaleRepository : CrudRepositoryBase<Sale, int, DbSale>, ISaleRepository
{
    public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    protected override string TableName => "PRODEJE";
    protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbSale.prodej_id) };
    protected override Sale MapToEntity(DbSale dbEntity)
    {
        throw new NotImplementedException();
    }

    protected override DbSale MapToDbEntity(Sale entity)
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