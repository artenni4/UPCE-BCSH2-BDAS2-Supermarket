using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoredProducts;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.SoldProducts;

namespace Supermarket.Infrastructure.StoredProducts
{
    internal class StoredProductRepository : CrudRepositoryBase<StoredProduct, int, SoldProductRepository.DbSoldProduct, PagingQueryObject>, IStoredProductRepository
    {
        public StoredProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbStoredProduct
        {
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
        
        protected override StoredProduct MapToEntity(SoldProductRepository.DbSoldProduct dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override SoldProductRepository.DbSoldProduct MapToDbEntity(StoredProduct entity)
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
