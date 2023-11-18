using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Infrastructure.Products;

public class DbBestSellingProduct
{
    public required int zbozi_id { get; init; }
    public required string nazev { get; init; }
    public required int pocet_prodeje { get; init; }

    public BestSellingProduct ToDomainEntity() => new BestSellingProduct
    {
        ProductId = zbozi_id,
        Name = nazev,
        SoldCount = pocet_prodeje
    };
}