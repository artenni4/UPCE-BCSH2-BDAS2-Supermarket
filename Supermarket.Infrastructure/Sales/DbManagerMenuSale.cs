using Supermarket.Core.UseCases.ManagerMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Sales
{
    public class DbManagerMenuSale
    {
        public required int prodej_id { get; init; }
        public required string zbozi { get; init; }
        public required DateTime datum { get; init; }
        public required decimal? cena { get; init; }
        public required string? typ_placeni_nazev { get; init; }
        public required int pokladna_id { get; init; }
        public required string pokladna_nazev { get; init; }

        public static IReadOnlyList<string> IdentityColumns { get; } = new[]
        {
            nameof(prodej_id)
        };

        public ManagerMenuSale ToDomainEntity() => new()
        {
            Id = prodej_id,
            Products = zbozi,
            Date = datum,
            Price = cena,
            PaymentType = typ_placeni_nazev,
            CashboxId = pokladna_id,
            CashboxName = pokladna_nazev
        };
    }
}
