using Dapper;
using Supermarket.Domain.StoragePlaces;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class DbStoragePlace : IDbEntity<StoragePlace, int, DbStoragePlace>
{
    public required int misto_ulozeni_id { get; init; }
    public required string kod { get; init; }
    public string? poloha { get; init; }
    public required int supermarket_id { get; init; }
    public required StoragePlaceType misto_ulozeni_typ { get; init; }
    
    public static string TableName => "MISTA_ULOZENI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(misto_ulozeni_id)
    };

    public StoragePlace ToDomainEntity()
    {
        return new StoragePlace
        {
            Id = misto_ulozeni_id,
            Code = kod,
            Location = poloha,
            SupermarketId = supermarket_id,
            Type = misto_ulozeni_typ
        };
    }

    public static DbStoragePlace MapToDbEntity(StoragePlace entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@misto_ulozeni_id", id);

        return parameters;
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