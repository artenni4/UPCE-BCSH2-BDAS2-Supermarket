using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Core.GoodsKeeping;

public class GoodsKeepingStoragePlace
{
    public required int StoragePlaceId { get; init; }
    public required string Code { get; init; }

    public static GoodsKeepingStoragePlace FromStoragePlace(StoragePlace stPlace) => new()
    {
        StoragePlaceId = stPlace.Id,
        Code = stPlace.Code
    };
}