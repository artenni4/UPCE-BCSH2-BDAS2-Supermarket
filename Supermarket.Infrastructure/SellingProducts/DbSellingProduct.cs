using Dapper;
using Supermarket.Core.Domain.SellingProducts;

namespace Supermarket.Infrastructure.SellingProducts;

internal class DbSellingProduct : IDbEntity<SellingProduct, SellingProductId, DbSellingProduct>
{
    public required int zbozi_id { get; init; }
    public required int supermarket_id { get; init; }
    
    
    public static string TableName => "PRODAVANE_ZBOZI";
    
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(zbozi_id),
        nameof(supermarket_id)
    };

    public SellingProduct ToDomainEntity() => new()
    {
        Id = new SellingProductId(zbozi_id, supermarket_id)
    };

    public static DbSellingProduct ToDbEntity(SellingProduct entity) => new()
    {
        supermarket_id = entity.Id.SupermarketId,
        zbozi_id = entity.Id.ProductId
    };

    public static DynamicParameters GetEntityIdParameters(SellingProductId id) =>
        new DynamicParameters()
            .AddParameter(nameof(supermarket_id), id.SupermarketId)
            .AddParameter(nameof(zbozi_id), id.ProductId);

    public static DynamicParameters GetOutputIdentityParameters() =>
        new DynamicParameters()
            .AddOutputParameter(nameof(supermarket_id))
            .AddOutputParameter(nameof(zbozi_id));

    public static SellingProductId ExtractIdentity(DynamicParameters dynamicParameters) => new(
        ProductId: dynamicParameters.Get<int>(nameof(zbozi_id)),
        SupermarketId: dynamicParameters.Get<int>(nameof(supermarket_id)));
}