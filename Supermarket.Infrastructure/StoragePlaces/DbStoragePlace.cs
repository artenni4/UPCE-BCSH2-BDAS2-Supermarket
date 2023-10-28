using Dapper;
using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class DbStoragePlace : IDbEntity<StoragePlace, int, DbStoragePlace>
{
    public required int misto_ulozeni_id { get; init; }
    
    public static string TableName => "MISTA_ULOZENI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(misto_ulozeni_id)
    };

    public StoragePlace ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbStoragePlace MapToDbEntity(StoragePlace entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(int id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }
}