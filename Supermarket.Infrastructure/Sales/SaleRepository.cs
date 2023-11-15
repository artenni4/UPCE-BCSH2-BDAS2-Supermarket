using Dapper;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Infrastructure.Employees;
using Supermarket.Infrastructure.StoredProducts;

namespace Supermarket.Infrastructure.Sales
{
    internal class SaleRepository : CrudRepositoryBase<Sale, int, DbSale>, ISaleRepository
    {
        public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public async Task<int> AddAndGetIdAsync(Sale sale)
        {
            var dbEntity = DbSale.ToDbEntity(sale);
            var insertingValues = dbEntity.GetInsertingValues();
            
            var selector = string.Join(", ", insertingValues.ParameterNames);
            var parameters = string.Join(", ", insertingValues.ParameterNames.Select(v => ":" + v));
            
            var sql = $"INSERT INTO {DbSale.TableName} ({selector}) VALUES ({parameters}) RETURNING prodej_id INTO :prodej_id";

            insertingValues.Add(nameof(DbSale.prodej_id), dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);

            return insertingValues.Get<int>(nameof(DbSale.prodej_id));
        }

        public async Task<PagedResult<ManagerMenuSale>> GetSupermarketSales(int supermarketId, DateTime dateFrom, DateTime dateTo, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("supermarket_id", supermarketId).AddParameter("datum_od", dateFrom).AddParameter("datum_do", dateTo);

            const string sql = @"SELECT
                                    p.prodej_id,
                                    p.datum,
                                    pk.pokladna_id,
                                    pk.nazev as pokladna_nazev,
                                    SUM(pl.castka) as cena,
                                    LISTAGG(DISTINCT tp.nazev, ', ') WITHIN GROUP (ORDER BY tp.nazev) AS typ_placeni_nazev,
                                    LISTAGG(DISTINCT z.nazev || ' ' || pz.kusy || mj.zkratka || ' ' || pz.celkova_cena || 'Kč', ', ') WITHIN GROUP (ORDER BY z.nazev) AS zbozi
                                FROM
                                    PRODEJE p
                                JOIN
                                    POKLADNY pk ON pk.pokladna_id = p.pokladna_id
                                JOIN
                                    PLATBA pl ON pl.prodej_id = p.prodej_id
                                JOIN
                                    TYPY_PLACENI tp ON tp.typ_placeni_id = pl.typ_placeni_id
                                JOIN
                                    PRODANE_ZBOZI pz ON pz.prodej_id = p.prodej_id
                                JOIN
                                    ZBOZI z ON z.zbozi_id = pz.zbozi_id
                                JOIN
                                    MERNE_JEDNOTKY mj ON z.merna_jednotka_id = mj.merna_jednotka_id
                                WHERE pk.supermarket_id = :supermarket_id AND p.datum BETWEEN :datum_od AND :datum_do
                                GROUP BY
                                    p.prodej_id,
                                    p.datum,
                                    pk.pokladna_id,
                                    pk.nazev";

            var orderByColumns = DbManagerMenuSale.IdentityColumns
            .Select(ic => $"p.{ic}");

            var result = await GetPagedResult<DbManagerMenuSale>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }
    }
}
