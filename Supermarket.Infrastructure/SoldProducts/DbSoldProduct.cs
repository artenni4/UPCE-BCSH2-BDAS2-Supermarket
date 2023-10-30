using Dapper;
using Supermarket.Domain.SoldProducts;

namespace Supermarket.Infrastructure.SoldProducts;

internal class DbSoldProduct : IDbEntity<SoldProduct, SoldProductId, DbSoldProduct>
{
    public required int prodej_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
    
    public static string TableName => "PRODANE_ZBOZI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(prodej_id),
        nameof(supermarket_id),
        nameof(zbozi_id)
    };

    public SoldProduct ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbSoldProduct MapToDbEntity(SoldProduct entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(SoldProductId id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static SoldProductId ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}