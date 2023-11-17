using Supermarket.Core.Domain.StoragePlaces;

namespace Supermarket.Core.UseCases.GoodsKeeping;

public class GoodsKeepingStoragePlace
{
    public required int Id { get; init; }
    public required string Code { get; init; }

    public static GoodsKeepingStoragePlace FromStoragePlace(StoragePlace stPlace) => new()
    {
        Id = stPlace.Id,
        Code = stPlace.Code
    };
}