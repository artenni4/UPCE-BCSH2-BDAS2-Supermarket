using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Sales;

namespace Supermarket.Infrastructure.Sales
{
    internal class SaleRepository : CrudRepositoryBase<Sale, int, DbSale>, ISaleRepository
    {
        public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }
    }
}
