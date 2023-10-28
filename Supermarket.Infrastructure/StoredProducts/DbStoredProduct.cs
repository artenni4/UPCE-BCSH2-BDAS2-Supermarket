using Dapper;
using Supermarket.Domain.StoredProducts;

namespace Supermarket.Infrastructure.StoredProducts;

internal class DbStoredProduct : IDbEntity<StoredProduct, StoredProductId, DbStoredProduct>
{
    public required int misto_ulozeni_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
    
    
    public static string TableName => "ULOZENI_ZBOZI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(misto_ulozeni_id),
        nameof(supermarket_id),
        nameof(zbozi_id)
    };

    public StoredProduct ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbStoredProduct MapToDbEntity(StoredProduct entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(StoredProductId id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static StoredProductId ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}