using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Products;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.Products
{
    internal class ProductRepository : CrudRepositoryBase<Product, int, ProductRepository.DbProduct, ProductQueryObject>, IProductRepository
    {
        public ProductRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbProduct
        {
            public required int zbozi_id { get; init; }
        }

        protected override string TableName => "ZBOZI";
        protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbProduct.zbozi_id) };
        protected override Product MapToEntity(DbProduct dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbProduct MapToDbEntity(Product entity)
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

        protected override string? GetCustomPagingCondition(ProductQueryObject queryObject, out DynamicParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
