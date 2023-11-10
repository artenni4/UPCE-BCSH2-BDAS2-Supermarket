namespace Supermarket.Core.UseCases.GoodsKeeping;

public class GoodsKeepingStoredProduct
{
    public required int ProductId { get; init; }
    public required string ProductName { get; init; }
    public required decimal Count { get; init; }
    public required int StoragePlaceId { get; init; }
    public required string StoragePlaceCode { get; init; }

}