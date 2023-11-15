using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Sales;

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
    }
}
