namespace Supermarket.Core.GoodsKeeping;

public class MovingProduct
{
    public required int ProductId { get; init; }
    public required decimal Count { get; init; }
    public required int NewStoragePlaceId { get; init; }
}