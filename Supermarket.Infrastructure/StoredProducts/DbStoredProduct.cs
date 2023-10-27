namespace Supermarket.Infrastructure.StoredProducts;

internal class DbStoredProduct
{
    public required int misto_ulozeni_id { get; init; }
    public required int supermarket_id { get; init; }
    public required int zbozi_id { get; init; }
}