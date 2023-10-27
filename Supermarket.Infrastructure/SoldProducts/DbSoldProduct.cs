namespace Supermarket.Infrastructure.SoldProducts;

internal class DbSoldProduct
{
    public required int prodej_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
}