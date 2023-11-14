using Dapper;
using Supermarket.Core.Domain.StoragePlaces;

namespace Supermarket.Infrastructure.StoragePlaces;

internal class DbStoragePlace : IDbEntity<StoragePlace, int, DbStoragePlace>
{
    public required int misto_ulozeni_id { get; init; }
    public required string kod { get; init; }
    public string? poloha { get; init; }
    public required int supermarket_id { get; init; }
    public required string misto_ulozeni_typ { get; init; }
    
    public static string TableName => "MISTA_ULOZENI";

    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(misto_ulozeni_id)
    };

    public StoragePlace ToDomainEntity()
    {
        if (Enum.TryParse(typeof(StoragePlaceType), misto_ulozeni_typ, out var storagePlaceType))
        {
            return new StoragePlace
            {
                Id = misto_ulozeni_id,
                Code = kod,
                Location = poloha,
                SupermarketId = supermarket_id,
                Type = (StoragePlaceType)storagePlaceType
            };
        }
        else
        {
            throw new InvalidOperationException($"Invalid value for StoragePlaceType: {misto_ulozeni_typ}");
        }
    }


    public static DbStoragePlace ToDbEntity(StoragePlace entity) => new()
    {
        misto_ulozeni_id = entity.Id,
        kod = entity.Code,
        poloha = entity.Location,
        supermarket_id = entity.SupermarketId,
        misto_ulozeni_typ = entity.Type.ToString()
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters()
            .AddParameter(nameof(misto_ulozeni_id), id);

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static int ExtractIdentity(DynamicParameters dynamicParameters)
    {
        throw new NotImplementedException();
    }

    public DynamicParameters GetInsertingValues() =>
        new DynamicParameters().AddParameter(nameof(kod), kod).AddParameter(nameof(misto_ulozeni_typ), misto_ulozeni_typ).AddParameter(nameof(supermarket_id), supermarket_id).AddParameter(nameof(poloha), poloha);

    public DynamicParameters GetUpdateValues() =>
        new DynamicParameters().AddParameter(nameof(misto_ulozeni_id), misto_ulozeni_id).AddParameter(nameof(kod), kod).AddParameter(nameof(misto_ulozeni_typ), misto_ulozeni_typ).AddParameter(nameof(supermarket_id), supermarket_id).AddParameter(nameof(poloha), poloha);

}