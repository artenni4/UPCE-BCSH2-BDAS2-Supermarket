using Supermarket.Core.Domain.Products;

namespace Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;

public class SuppliedProductModel
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public required decimal Count { get; init; }
    public required MeasureUnit MeasureUnit { get; init; }
    public required bool IsByWeight { get; init; }
}