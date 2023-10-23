namespace Supermarket.Core.GoodsKeeping;

public class StoredProduct
{
    public required int ProductId { get; init; }
    public required int ProductName { get; init; }
    public required decimal Count { get; init; }
    public required int StoragePlaceId { get; init; }
    public required string StoragePlaceCode { get; init; }
}