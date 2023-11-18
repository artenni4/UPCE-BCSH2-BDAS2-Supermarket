namespace Supermarket.Core.UseCases.ManagerMenu;

public class BestSellingProduct
{
    public required int ProductId { get; init; }
    public required string Name { get; init; }
    public required int SoldCount { get; init; }
}