using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.SoldProducts;

namespace Supermarket.Infrastructure.StoredProducts;

internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, int, DbSoldProduct>, IStoredProductRepository
{
    public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    protected override string TableName => "ULOZENI_ZBOZI";

    protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(DbStoredProduct.misto_ulozeni_id),
        nameof(DbStoredProduct.supermarket_id),
        nameof(DbStoredProduct.zbozi_id)
    };
        
    protected override StoredProduct MapToEntity(DbSoldProduct dbEntity)
    {
        throw new NotImplementedException();
    }

    protected override DbSoldProduct MapToDbEntity(StoredProduct entity)
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