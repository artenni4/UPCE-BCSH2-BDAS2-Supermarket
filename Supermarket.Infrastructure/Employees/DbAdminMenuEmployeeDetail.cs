using Supermarket.Core.UseCases.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Employees
{
    internal class DbAdminMenuEmployeeDetail
    {
        public required int zamestnanec_id { get; init; }
        public required int? manazer_id { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required string login { get; init; }
        public required DateTime datum_nastupu { get; init; }
        public required int? supermarket_id { get; init; }
        public required bool isPokladnik { get; init; }
        public required bool isNakladac { get; init; }
        public required bool isManazer { get; init; }
        public required bool isAdmin { get; init; }


        public static string TableName => "ZAMESTNANCI";
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(zamestnanec_id)
        };

        public AdminEmployeeDetail ToDomainEntity() => new()
        {
            Id = zamestnanec_id,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            SupermarketId = supermarket_id,
            IsAdmin = isAdmin,
            IsCashier = isPokladnik,
            IsGoodsKeeper = isNakladac,
            IsManager = isManazer,
            Login = login,
            ManagerId = manazer_id
        };
    }
}
