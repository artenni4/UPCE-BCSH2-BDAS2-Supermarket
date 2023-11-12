using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Infrastructure.ProductCategories;
using Supermarket.Infrastructure.Products;

namespace Supermarket.Infrastructure.SellingProducts;

internal class SellingProductRepository : CrudRepositoryBase<SellingProduct, SellingProductId, DbSellingProduct>, ISellingProductRepository
{
    public SellingProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }

    public async Task<PagedResult<Product>> GetSupermarketProducts(int supermarketId, RecordsRange recordsRange, int categoryId, string? searchText)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId)
            .AddParameter("druh_zbozi_id", categoryId)
            .AddParameter("hledani", searchText);
        
        const string sql = @"SELECT z.* FROM ZBOZI z
                     JOIN PRODAVANE_ZBOZI pz ON (z.zbozi_id = pz.zbozi_id)
                     WHERE pz.supermarket_id = :supermarket_id AND z.druh_zbozi_id = :druh_zbozi_id AND z.nazev LIKE '%' || :hledani || '%'";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");
        
        var result = await GetPagedResult<DbProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<PagedResult<ManagerMenuProduct>> GetAllSupermarketProducts(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);

        const string sql = @"SELECT
                                z.zbozi_id as zbozi_id,
                                z.nazev as nazev,
                                z.cena as cena,
                                d.dodavatel_id as dodavatel_id,
                                d.nazev as dodavatel_nazev,
                                SUM(uz.kusy) as kusy
                            FROM
                                PRODAVANE_ZBOZI pz
                            JOIN
                                ZBOZI z ON z.zbozi_id = pz.zbozi_id
                            JOIN
                                DODAVATELE d ON z.dodavatel_id = d.dodavatel_id
                            LEFT JOIN
                                ULOZENI_ZBOZI uz ON z.zbozi_id = uz.zbozi_id and uz.supermarket_id = :supermarket_id
                            WHERE
                                pz.supermarket_id = :supermarket_id
                            GROUP BY
                                z.zbozi_id, z.nazev, z.cena, d.dodavatel_id, d.nazev";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");

        var result = await GetPagedResult<DbManagerMenuProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<PagedResult<ProductCategory>> GetSupermarketProductCategories(int supermarketId, RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);
        
        const string sql = @"SELECT DISTINCT dz.*
                            FROM DRUHY_ZBOZI dz
                            JOIN ZBOZI z ON dz.DRUH_ZBOZI_ID = z.DRUH_ZBOZI_ID
                            JOIN PRODAVANE_ZBOZI pz ON z.ZBOZI_ID = pz.ZBOZI_ID
                            WHERE pz.SUPERMARKET_ID = :supermarket_id";

        var orderByColumns = DbProductCategory.IdentityColumns
            .Select(ic => $"dz.{ic}");
        
        var result = await GetPagedResult<DbProductCategory>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProductCategory => dbProductCategory.ToDomainEntity());
    }

    private class DbManagerMenuProduct
    {
        public required int zbozi_id { get; set; }
        public required string nazev { get; set; }
        public required decimal kusy { get; set; }
        public required int dodavatel_id { get; set; }
        public required string dodavatel_nazev { get; set; }
        public required decimal cena { get; set; }

        public ManagerMenuProduct ToDomainEntity() => new ManagerMenuProduct
        {
            ProductId = zbozi_id,
            ProductName = nazev,
            Count = kusy,
            SupplierId = dodavatel_id,
            SupplierName = dodavatel_nazev,
            Price = cena
        };
    }
}