using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Sales;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Sales
{
    internal class SaleRepository : CrudRepositoryBase<Sale, int, SaleRepository.DbSale, PagingQueryObject>, ISaleRepository
    {
        public SaleRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }
        
        public class DbSale
        {
            public required int prodej_id { get; init; }
        }

        protected override string TableName => "PRODEJE";
        protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbSale.prodej_id) };
        protected override Sale MapToEntity(DbSale dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbSale MapToDbEntity(Sale entity)
        {
            throw new NotImplementedException();
        }

        protected override DynamicParameters GetIdentityValues(int id)
        {
            throw new NotImplementedException();
        }

        protected override int ExtractIdentity(DynamicParameters dynamicParameters)
        {
            throw new NotImplementedException();
        }
    }
}
