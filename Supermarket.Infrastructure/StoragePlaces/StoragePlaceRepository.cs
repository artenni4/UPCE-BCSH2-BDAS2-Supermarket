﻿using Dapper;
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
        }

        protected override string TableName => "MISTA_ULOZENI";

        protected override IReadOnlyList<string> IdentityColumns { get; } =
            new[] { nameof(DbStoragePlace.misto_ulozeni_id) };
        
        protected override StoragePlace MapToEntity(DbStoragePlace dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbStoragePlace MapToDbEntity(StoragePlace entity)
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
