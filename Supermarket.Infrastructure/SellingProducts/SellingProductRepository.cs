using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.SellingProducts;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.SellingProducts
{
    internal class SellingProductRepository : CrudRepositoryBase<SellingProduct, int, SellingProductRepository.DbSellingProduct, PagingQueryObject>, ISellingProductRepository
    {
        public SellingProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbSellingProduct
        {
            public required int zbozi_id { get; init; }
            public required int supermarket_id { get; init; }
        }

        protected override string TableName => "PRODAVANE_ZBOZI";

        protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
            { nameof(DbSellingProduct.zbozi_id), nameof(DbSellingProduct.supermarket_id) };
        
        protected override SellingProduct MapToEntity(DbSellingProduct dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbSellingProduct MapToDbEntity(SellingProduct entity)
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
