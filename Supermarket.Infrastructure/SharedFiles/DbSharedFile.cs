using Dapper;
using Supermarket.Core.Domain.SharedFiles;

namespace Supermarket.Infrastructure.SharedFiles
{
    public class DbSharedFile : IDbEntity<SharedFile, int, DbSharedFile>
    {
        public required int soubor_id { get; init; }
        public required string nazev_souboru { get; init; }
        public required string pripona { get; init; }
        public required DateTime datum_nahrani { get; init; }
        public required DateTime? datum_modifikace { get; init; }
        public required int supermarket_id { get; init; }
        public required int zamestnanec_id { get; init; }


        public static string TableName => "SOUBORY";

        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(soubor_id)
        };

        public SharedFile ToDomainEntity() => new()
        {
            Id = soubor_id,
            CreatedDate = datum_nahrani,
            EmployeeId = zamestnanec_id,
            Extenstion = pripona,
            ModifiedDate = datum_modifikace,
            Name = nazev_souboru,
            SupermarketId = supermarket_id
        };

        public static DbSharedFile ToDbEntity(SharedFile entity) => new()
        {
            soubor_id = entity.Id,
            datum_modifikace = entity.ModifiedDate,
            datum_nahrani = entity.CreatedDate,
            nazev_souboru = entity.Name,
            pripona = entity.Extenstion,
            supermarket_id = entity.SupermarketId,
            zamestnanec_id = entity.EmployeeId,
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(soubor_id), id);


        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }

}