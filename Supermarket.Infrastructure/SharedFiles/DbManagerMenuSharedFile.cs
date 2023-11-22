using Dapper;
using Supermarket.Core.Domain.SharedFiles;
using Supermarket.Core.UseCases.ManagerMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.SharedFiles
{
    public class DbManagerMenuSharedFile : IDbEntity<ManagerMenuSharedFile, int, DbManagerMenuSharedFile>
    {
        public required int soubor_id { get; init; }
        public required string nazev_souboru { get; init; }
        public required string pripona { get; init; }
        public required DateTime datum_nahrani { get; init; }
        public required DateTime? datum_modifikace { get; init; }
        public required int supermarket_id { get; init; }
        public required int zamestnanec_id { get; init; }
        public required string zamestnanec_nazev { get; init; }

        public static string TableName => "SOUBORY";

        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(soubor_id)
        };

        public ManagerMenuSharedFile ToDomainEntity() => new()
        {
            Id = soubor_id,
            CreatedDate = datum_nahrani,
            EmployeeId = zamestnanec_id,
            EmployeeName = zamestnanec_nazev,
            Extenstion = pripona,
            ModifiedDate = datum_modifikace,
            Name = nazev_souboru,
            SupermarketId = supermarket_id,
        };

        public static DbManagerMenuSharedFile ToDbEntity(ManagerMenuSharedFile entity) => new()
        {
            soubor_id = entity.Id,
            datum_modifikace = entity.ModifiedDate,
            datum_nahrani = entity.CreatedDate,
            nazev_souboru = entity.Name,
            pripona = entity.Extenstion,
            supermarket_id = entity.SupermarketId,
            zamestnanec_id = entity.EmployeeId,
            zamestnanec_nazev = entity.EmployeeName,
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(soubor_id), id);


        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }
}
