using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.SoldProducts;

namespace Supermarket.Infrastructure.SoldProducts;

internal class SoldProductRepository : CrudRepositoryBase<SoldProduct, SoldProductId, DbSoldProduct>, ISoldProductRepository
{
    public SoldProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}