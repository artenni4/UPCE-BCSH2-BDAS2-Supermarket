using Dapper;

namespace Supermarket.Infrastructure.Supermarkets;

internal class DbSupermarket : IDbEntity<Domain.Supermarkets.Supermarket, int, DbSupermarket>
{
    public required int supermarket_id { get; init; }
    
    public static string TableName => "SUPERMARKETY";
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(supermarket_id)
    };

    public Domain.Supermarkets.Supermarket ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbSupermarket MapToDbEntity(Domain.Supermarkets.Supermarket entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(int id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}