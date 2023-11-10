using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products;

internal class ProductRepository : CrudRepositoryBase<Product, int, DbProduct>, IProductRepository
{
    public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<ManagerMenuAddProduct>> GetManagerProductsToAdd(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);

        const string sql = @"SELECT z.zbozi_id as zbozi_id, z.nazev as nazev, z.cena as cena,
                            CASE WHEN uz.zbozi_id IS NOT NULL THEN 1 ELSE 0 END AS is_in_supermarket,
                            d.nazev AS dodavatel_nazev, d.dodavatel_id as dodavatel_id
                        FROM
                            ZBOZI z
                        LEFT JOIN
                            ULOZENI_ZBOZI uz ON z.zbozi_id = uz.zbozi_id
                        LEFT JOIN
                            DODAVATELE d ON z.dodavatel_id = d.dodavatel_id";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");

        var result = await GetPagedResult<DbManagerMenuAddProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    private class DbManagerMenuAddProduct
    {
        public required int zbozi_id { get; set; }
        public required string nazev { get; set; }
        public required bool is_in_supermarket { get; set; }
        public required int dodavatel_id { get; set; }
        public required string dodavatel_nazev { get; set; }
        public required decimal cena { get; set; }


        public ManagerMenuAddProduct ToDomainEntity() => new ManagerMenuAddProduct
        {
            ProductId = zbozi_id,
            ProductName = nazev,
            IsInSupermarket = is_in_supermarket,
            Price = cena,
            SupplierId = dodavatel_id,
            SupplierName = dodavatel_nazev
        };
    }
}