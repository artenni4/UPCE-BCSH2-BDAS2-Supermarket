using Dapper;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.ManagerMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Employees 
{
    internal class DbManagerMenuEmployeeDetail : IDbEntity<ManagerMenuEmployeeDetail, int, DbManagerMenuEmployeeDetail>
    {
        public required int zamestnanec_id { get; init; }
        public required string login { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required DateTime datum_nastupu { get; init; }
        public required int manazer_id { get; init; }
        public required int supermarket_id { get; init; }
        public required bool isPokladnik { get; init; }
        public required bool isNakladac { get; init; }
        public required bool isManazer { get; init; }

        public static string TableName => "ZAMESTNANCI";
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(zamestnanec_id)
        };

        public ManagerMenuEmployeeDetail ToDomainEntity() => new()
        {
            Id = zamestnanec_id,
            Login = login,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            ManagerId = manazer_id,
            SupermarketId = supermarket_id,
            IsCashier = isPokladnik,
            IsGoodsKeeper = isNakladac,
            IsManager = isManazer,
        };

        public static DbManagerMenuEmployeeDetail ToDbEntity(ManagerMenuEmployeeDetail entity) => new()
        {
            zamestnanec_id = entity.Id,
            login = entity.Login,
            jmeno = entity.Name,
            prijmeni = entity.Surname,
            datum_nastupu = entity.HireDate,
            isPokladnik = entity.IsCashier,
            isNakladac = entity.IsGoodsKeeper,
            isManazer = entity.IsManager,
            manazer_id = entity.ManagerId,
            supermarket_id = entity.SupermarketId
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(zamestnanec_id), id);

        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }
}
