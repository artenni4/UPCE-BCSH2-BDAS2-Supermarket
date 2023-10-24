using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.SoldProducts;
using Supermarket.Infrastructure.Common;


namespace Supermarket.Infrastructure.SoldProducts
{
    internal class SoldProductRepository : CrudRepositoryBase<SoldProduct, int, SoldProductRepository.DbSoldProduct, PagingQueryObject>, ISoldProductRepository
    {
        public SoldProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbSoldProduct
        {
            public required int prodej_id { get; init; }
            public required int supermarket_id { get; init; }
            public required int zbozi_id { get; init; }
        }

        protected override string TableName => "PRODANE_ZBOZI";

        protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
            { nameof(DbSoldProduct.prodej_id), nameof(DbSoldProduct.supermarket_id), nameof(DbSoldProduct.zbozi_id) };
        
        protected override SoldProduct MapToEntity(DbSoldProduct dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbSoldProduct MapToDbEntity(SoldProduct entity)
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

        protected override string? GetCustomPagingCondition(PagingQueryObject queryObject, out DynamicParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
