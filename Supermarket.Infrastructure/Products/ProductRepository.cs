using System.Data;
using Dapper;
using Dapper.Oracle;
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

        const string sql = @"SELECT * FROM ADMINPRODUCTSVIEW";

        var orderByColumns = DbProduct.IdentityColumns
            .Select(ic => $"ADMINPRODUCTSVIEW.{ic}");

        var result = await GetPagedResult<DbAdminProduct>(recordsRange, sql, orderByColumns, parameters);

        return result.Select(dbProduct => dbProduct.ToDomainEntity());
    }

    public async Task<BestSellingProduct?> GetBestSellingProduct(int supermarketId)
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("p_supermarket_id", supermarketId, direction: ParameterDirection.Input);
        parameters.Add("zbozi_rc", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        var sellingProduct = await _oracleConnection
            .QuerySingleOrDefaultAsync<DbBestSellingProduct>("DEJ_NEJPRODAVANEJSI_ZBOZI", parameters, commandType: CommandType.StoredProcedure);
        
        return sellingProduct?.ToDomainEntity();
    }
}