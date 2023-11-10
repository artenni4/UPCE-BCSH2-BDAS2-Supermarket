using Supermarket.Core.Domain.StoragePlaces;

namespace Supermarket.Core.UseCases.GoodsKeeping;

public class SupplyWarehouse
{
    public required int Id { get; init; }
    public required string Code { get; init; }

    public static SupplyWarehouse FromStoragePlace(StoragePlace stPlace) => new()
    {
        Id = stPlace.Id,
        Code = stPlace.Code
    };
}