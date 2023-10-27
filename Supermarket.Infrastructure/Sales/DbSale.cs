using Dapper;
using Supermarket.Domain.Sales;

namespace Supermarket.Infrastructure.Sales;

internal class DbSale : IDbEntity<Sale, int, DbSale>
{
    public required int prodej_id { get; init; }
    
    
    public static string TableName => "PRODEJE";
    
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(prodej_id)
    };

    public Sale ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbSale MapToDbEntity(Sale entity)
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