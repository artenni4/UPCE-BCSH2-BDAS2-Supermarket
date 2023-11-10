using Dapper;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.StoredProducts;

internal class DbStoredProduct : IDbEntity<StoredProduct, StoredProductId, DbStoredProduct>
{
    public required int misto_ulozeni_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
    public required decimal kusy { get; init; }
    
    public static string TableName => "ULOZENI_ZBOZI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(misto_ulozeni_id),
        nameof(supermarket_id),
        nameof(zbozi_id),
    };

    public StoredProduct ToDomainEntity()
    {
        return new StoredProduct
        {
            Id = new StoredProductId(misto_ulozeni_id, supermarket_id, zbozi_id),
            Count = kusy
        };
    }

    public static DbStoredProduct ToDbEntity(StoredProduct entity) => new()
    {
        zbozi_id = entity.Id.ProductId,
        supermarket_id = entity.Id.SupermarketId,
        misto_ulozeni_id = entity.Id.StoragePlaceId,
        kusy = entity.Count
    };

    public static DynamicParameters GetEntityIdParameters(StoredProductId id) =>
        new DynamicParameters().AddParameter(nameof(zbozi_id), id.ProductId).AddParameter(nameof(supermarket_id), id.SupermarketId).AddParameter(nameof(misto_ulozeni_id), id.StoragePlaceId);

    public static StoredProductId ExtractIdentity(DynamicParameters dynamicParameters)
    {
        int storagePlaceId = dynamicParameters.Get<int>(nameof(misto_ulozeni_id));
        int supermarketId = dynamicParameters.Get<int>(nameof(supermarket_id));
        int productId = dynamicParameters.Get<int>(nameof(zbozi_id));

        return new StoredProductId(storagePlaceId, supermarketId, productId);
    }

    public DynamicParameters GetInsertingValues() =>
        new DynamicParameters().AddParameter(nameof(zbozi_id), zbozi_id).AddParameter(nameof(supermarket_id), supermarket_id).AddParameter(nameof(misto_ulozeni_id), misto_ulozeni_id).AddParameter(nameof(kusy), kusy);

}