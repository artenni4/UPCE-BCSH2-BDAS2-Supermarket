using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.SoldProducts;

namespace Supermarket.Infrastructure.StoredProducts;

internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, StoredProductId, DbStoredProduct>, IStoredProductRepository
{
    public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}