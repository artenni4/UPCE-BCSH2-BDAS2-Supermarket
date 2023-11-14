using Dapper;
using Supermarket.Core.Domain.SellingProducts;
using System.Collections.Generic;

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

    public DynamicParameters GetInsertingValues() =>
        new DynamicParameters().AddParameter(nameof(zbozi_id), zbozi_id).AddParameter(nameof(supermarket_id), supermarket_id);
}


