using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu.Db
{
    public class DbManagerMenuAddProduct
    {
        public required int zbozi_id { get; set; }
        public required string nazev { get; set; }
        public required bool is_in_supermarket { get; set; }
        public required int dodavatel_id { get; set; }
        public required string dodavatel_nazev { get; set; }
        public required decimal cena { get; set; }
        public required int misto_ulozeni_id { get; set; }

        public ManagerMenuAddProduct ToDomainEntity() => new ManagerMenuAddProduct
        {
            ProductId = zbozi_id,
            ProductName = nazev,
            IsInSupermarket = is_in_supermarket,
            Price = cena,
            SupplierId = dodavatel_id,
            SupplierName = dodavatel_nazev,
            StoragePlaceId = misto_ulozeni_id
        };
    }
}
