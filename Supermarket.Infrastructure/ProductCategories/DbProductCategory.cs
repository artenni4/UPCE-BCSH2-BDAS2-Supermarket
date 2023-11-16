using Dapper;
using Supermarket.Core.Domain.ProductCategories;

namespace Supermarket.Infrastructure.ProductCategories;

internal class DbProductCategory : IDbEntity<ProductCategory, int, DbProductCategory>
{
    public required int druh_zbozi_id { get; init; }
    public required string nazev { get; init; }
    public required string? popis { get; init; }

    public static string TableName => "DRUHY_ZBOZI";

    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
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

    public static DbProductCategory ToDbEntity(ProductCategory entity) => new()
    {
        druh_zbozi_id = entity.Id,
        nazev = entity.Name,
        popis = entity.Description
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(druh_zbozi_id), id);

    public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
}