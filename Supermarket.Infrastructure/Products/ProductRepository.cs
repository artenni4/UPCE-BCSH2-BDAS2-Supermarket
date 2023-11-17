using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;

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

        const string sql = @"SELECT
                                z.zbozi_id as zbozi_id,
                                z.nazev as nazev,
                                z.cena as cena,
                                NVL(MAX(uz.misto_ulozeni_id), 0) as misto_ulozeni_id,
                                d.nazev AS dodavatel_nazev,
                                d.dodavatel_id as dodavatel_id,
                                pz.aktivni as is_in_supermarket
                            FROM
                                ZBOZI z
                            LEFT JOIN
                                PRODAVANE_ZBOZI pz ON z.zbozi_id = pz.zbozi_id AND pz.supermarket_id = :supermarket_id
                            LEFT JOIN
                                DODAVATELE d ON z.dodavatel_id = d.dodavatel_id
                            LEFT JOIN
                                ULOZENI_ZBOZI uz ON z.zbozi_id = uz.zbozi_id AND uz.supermarket_id = :supermarket_id
                            GROUP BY
                                z.zbozi_id, z.nazev, z.cena, d.nazev, d.dodavatel_id, pz.aktivni";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");

        var result = await GetPagedResult<DbManagerMenuAddProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<PagedResult<AdminProduct>> GetAdminProducts(RecordsRange recordsRange)
    {
        var parameters = new DynamicParameters();

        var sql = @"SELECT
                        z.zbozi_id as zbozi_id,
                        z.nazev as nazev,
                        z.navahu as navahu,
                        z.cena as cena,
                        z.carovykod as carovykod,
                        d.dodavatel_id as dodavatel_id,
                        d.nazev as dodavatel_nazev,
                        z.popis as popis,
                        z.merna_jednotka_id,
                        mj.nazev as merna_jednotka_nazev,
                        dz.druh_zbozi_id as druh_id,
                        dz.nazev as druh_nazev
                    FROM
                        ZBOZI z
                    JOIN
                        DODAVATELE d on d.dodavatel_id = z.dodavatel_id
                    JOIN
                        MERNE_JEDNOTKY mj on mj.merna_jednotka_id = z.merna_jednotka_id
                    JOIN
                        DRUHY_ZBOZI dz on dz.druh_zbozi_id = z.druh_zbozi_id";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"z.{ic}");

        var result = await GetPagedResult<DbAdminProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

}