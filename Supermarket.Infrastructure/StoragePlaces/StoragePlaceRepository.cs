using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.StoragePlaces;
using Supermarket.Infrastructure.Common;

namespace Supermarket.Infrastructure.StoragePlaces
{
    internal class StoragePlaceRepository : CrudRepositoryBase<StoragePlace, int, StoragePlaceRepository.DbStoragePlace, PagingQueryObject>, IStoragePlaceRepository
    {
        public StoragePlaceRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbStoragePlace
        {
            public required int misto_ulozeni_id { get; init; }
            public required string kod { get; init; }
            public string? poloha { get; init; }
            public int supermarket_id { get; init; }
            public required string misto_ulozeni_typ { get; init; }
        }

        protected override string TableName => "MISTA_ULOZENI";

        protected override IReadOnlyList<string> IdentityColumns { get; } =
            new[] { nameof(DbStoragePlace.misto_ulozeni_id) };

        protected override StoragePlace MapToEntity(DbStoragePlace dbEntity) => new()
        {
            Id = dbEntity.misto_ulozeni_id,
            Code = dbEntity.kod,
            Location = dbEntity.poloha,
            SupermarketId = dbEntity.supermarket_id,
            Type = dbEntity.misto_ulozeni_typ switch
            {
                
            }
        };

        protected override DbStoragePlace MapToDbEntity(StoragePlace entity) => new()
        {
            misto_ulozeni_id = entity.Id,
            kod = entity.Code,
            poloha = entity.Location,
            supermarket_id = entity.SupermarketId,
            misto_ulozeni_typ = entity.Type
        };

        protected override DynamicParameters GetIdentityValues(int id) => GetSimpleIdentityValue(id);

        protected override int ExtractIdentity(DynamicParameters dynamicParameters) => ExtractSimpleIdentity(dynamicParameters);
    }
}
