using Dapper;
using Supermarket.Domain.ProductCategories;

namespace Supermarket.Infrastructure.ProductCategories;

internal class DbProductCategory : IDbEntity<ProductCategory, int, DbProductCategory>
{
    public required int druh_zbozi_id { get; init; }
    public required string nazev { get; init; }
    public required string? popis { get; init; }

    public static string TableName => "DRUHY_ZBOZI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(druh_zbozi_id)
    };

    public ProductCategory ToDomainEntity()
    {
        return new ProductCategory
        {
            Id = druh_zbozi_id, 
            Name = nazev,
            Description = popis
        };
    }

    public static DbProductCategory MapToDbEntity(ProductCategory entity)
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