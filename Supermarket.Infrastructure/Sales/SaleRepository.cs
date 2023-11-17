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

            const string sql = @"SELECT * 
                                FROM 
                                    SALESVIEW
                                WHERE
                                    SALESVIEW.supermarket_id = :supermarket_id
                                    AND SALESVIEW.datum BETWEEN :datum_od AND :datum_do";

            var orderByColumns = DbManagerMenuSale.IdentityColumns
            .Select(ic => $"SALESVIEW.{ic}");

            var result = await GetPagedResult<DbManagerMenuSale>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }
    }
}
