using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.StoredProducts
{
    internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, int, StoredProductRepository.DbStoredProduct, PagingQueryObject>, IStoredProductRepository
    {
        public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbStoredProduct
        {
            public required decimal kusy { get; init; }
            public required int misto_ulozeni_id { get; init; }
            public required int supermarket_id { get; init; }
            public required int zbozi_id { get; init; }
        }

        protected override string TableName => "ULOZENI_ZBOZI";

        protected override IReadOnlyList<string> IdentityColumns { get; } = new[]
        {
            nameof(DbStoredProduct.misto_ulozeni_id),
            nameof(DbStoredProduct.supermarket_id),
            nameof(DbStoredProduct.zbozi_id)
        };

        protected override StoredProduct MapToEntity(DbStoredProduct dbEntity) => new()
        {
            Pieces = dbEntity.kusy,
            StoragePlaceId = dbEntity.misto_ulozeni_id,
            SupermarketId = dbEntity.supermarket_id,
            ProductId = dbEntity.zbozi_id
        };

        protected override DbStoredProduct MapToDbEntity(StoredProduct entity) => new()
        {
            kusy = entity.Pieces,
            misto_ulozeni_id = entity.StoragePlaceId,
            supermarket_id = entity.SupermarketId,
            zbozi_id = entity.ProductId
        };

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) =>
            ExtractSimpleIdentity(dynamicParameters);
    }
}
