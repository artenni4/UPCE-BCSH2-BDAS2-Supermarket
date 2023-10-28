using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Supermarkets;

namespace Supermarket.Infrastructure.Supermarkets;

internal class SupermarketRepository : CrudRepositoryBase<Domain.Supermarkets.Supermarket, int, DbSupermarket>, ISupermarketRepository
{
    public SupermarketRepository(OracleConnection oracleConnection) : base(oracleConnection)
    {
    }
}