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

        protected override SellingProduct MapToEntity(DbSellingProduct dbEntity) => new()
        {
            Id = dbEntity.zbozi_id,
            SupermarketId = dbEntity.supermarket_id
        };

        protected override DbSellingProduct MapToDbEntity(SellingProduct entity) => new()
        {
            zbozi_id = entity.Id,
            supermarket_id = entity.SupermarketId
        };

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) => ExtractSimpleIdentity(dynamicParameters);
    }
}
