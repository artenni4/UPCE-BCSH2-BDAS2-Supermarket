using Supermarket.Core.UseCases.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees.Roles;

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
        public required string rodne_cislo { get; init; }

        public static string TableName => "ZAMESTNANCI";
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(zamestnanec_id)
        };

        public AdminEmployeeDetail ToDomainEntity() => new AdminEmployeeDetail
        {
            Id = zamestnanec_id,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            SupermarketId = supermarket_id,
            Login = login,
            PersonalNumber = rodne_cislo,
            RoleInfo = GetRoleInfo()
        };

        private IEmployeeRoleInfo GetRoleInfo()
        {
            if (isAdmin)
            {
                return new Admin();
            }

            if (supermarket_id.HasValue == false)
            {
                throw new RepositoryInconsistencyException("Supermarket id is null, but employee is not admin.");
            }
            return new SupermarketEmployee(supermarket_id.Value, manazer_id, GetRoles());
        }
        
        private HashSet<SupermarketEmployeeRole> GetRoles()
        {
            var roles = new HashSet<SupermarketEmployeeRole>();
            if (isPokladnik)
            {
                roles.Add(SupermarketEmployeeRole.Cashier);
            }

            if (isNakladac)
            {
                roles.Add(SupermarketEmployeeRole.GoodsKeeper);
            }

            if (isManazer)
            {
                roles.Add(SupermarketEmployeeRole.Manager);
            }

            return roles;
        }
    }
}
