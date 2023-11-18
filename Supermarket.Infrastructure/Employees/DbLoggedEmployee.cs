using Dapper;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.Login;

namespace Supermarket.Infrastructure.Employees
{
    internal class DbLoggedEmployee
    {
        public required int zamestnanec_id { get; init; }
        public required string login { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required DateTime datum_nastupu { get; init; }
        public required string? rodne_cislo { get; init; }
        public required int? manazer_id { get; init; }
        public required int? supermarket_id { get; init; }
        public required bool isPokladnik { get; init; }
        public required bool isNakladac { get; init; }
        public required bool isManazer { get; init; }
        public required bool isAdmin { get; init; }
        public required byte[] heslo_hash { get; init; }
        public required byte[] heslo_salt { get; init; }

        public static string TableName => "ZAMESTNANCI";
        
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(zamestnanec_id)
        };

        public EmployeeRole ToDomainEntity() => new EmployeeRole
        {
            Id = zamestnanec_id,
            Login = login,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            PersonalNumber = rodne_cislo,
            RoleInfo = GetRoleInfo(),
            PasswordHash = heslo_hash,
            PasswordHashSalt = heslo_salt
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
