namespace Supermarket.Core.Products
{
    public class Product : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int ProductCategoryId { get; init; }
        public required MeasureUnit MeasureUnit { get; init; }
        public required bool ByWeight { get; init; }
        public required decimal Price { get; init; }
        public required decimal Weight { get; init; }
        public required string? Barcode {  get; init; }
        public required string? Description { get; init;}
    }
}
