using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Infrastructure.Products
{
    public class DbAdminProduct
    {
        public required int zbozi_id { get; set; }
        public required string nazev { get; set; }
        public required int druh_id { get; set; }
        public required string druh_nazev { get; set; }
        public required bool navahu { get; set; }
        public required decimal cena { get; set; }
        public required string carovykod { get; set; }
        public required int dodavatel_id { get; set; }
        public required string dodavatel_nazev { get; set; }
        public required int merna_jednotka_id { get; set; }
        public required string merna_jednotka_nazev { get; set; }
        public required string popis { get; set; }

        public AdminProduct ToDomainEntity() => new AdminProduct
        {
            Id = zbozi_id,
            Name = nazev,
            IsByWeight = navahu,
            Price = cena,
            Code = carovykod,
            SupplierId = dodavatel_id,
            SupplierName = dodavatel_nazev,
            Description = popis,
            CategoryId = druh_id,
            CategoryName = druh_nazev
        };
    }
}
