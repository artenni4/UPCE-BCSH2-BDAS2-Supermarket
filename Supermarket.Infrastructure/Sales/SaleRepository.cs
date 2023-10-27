using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Sales;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Sales;

internal class SaleRepository : CrudRepositoryBase<Sale, int, DbSale>, ISaleRepository
{
    public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}