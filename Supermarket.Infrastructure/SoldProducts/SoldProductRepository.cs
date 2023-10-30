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
            public required decimal kusy { get; init; }
            public required int supermarket_id { get; init; }
            public required int zbozi_id { get; init; }
        }

        protected override string TableName => "PRODANE_ZBOZI";

        protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
            { nameof(DbSoldProduct.prodej_id), nameof(DbSoldProduct.supermarket_id), nameof(DbSoldProduct.zbozi_id) };

        protected override SoldProduct MapToEntity(DbSoldProduct dbEntity) => new()
        {
            Id = dbEntity.prodej_id,
            Pieces = dbEntity.kusy,
            SupermarketId = dbEntity.supermarket_id,
            ProductId = dbEntity.zbozi_id
        };

        protected override DbSoldProduct MapToDbEntity(SoldProduct entity) => new()
        {
            prodej_id = entity.Id,
            kusy = entity.Pieces,
            supermarket_id = entity.SupermarketId,
            zbozi_id = entity.ProductId
        };

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) => ExtractSimpleIdentity(dynamicParameters);
    }
}
