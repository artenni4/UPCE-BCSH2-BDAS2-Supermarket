using Dapper;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.ManagerMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common;

namespace Supermarket.Infrastructure.Employees 
{
    internal class DbManagerMenuEmployeeDetail : IDbEntity<ManagerMenuEmployeeDetail, int, DbManagerMenuEmployeeDetail>
    {
        public required int zamestnanec_id { get; init; }
        public required string login { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required DateTime datum_nastupu { get; init; }
        public required int? manazer_id { get; init; }
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
            RoleInfo = new SupermarketEmployee(supermarket_id, manazer_id, GetRoles())
        };

        public static DbManagerMenuEmployeeDetail ToDbEntity(ManagerMenuEmployeeDetail entity)
        {
            if (entity.RoleInfo is not SupermarketEmployee supermarketEmployee)
            {
                throw new RepositoryInconsistencyException("Employee is not supermarket employee.");
            }
            
            return new DbManagerMenuEmployeeDetail
            {
                zamestnanec_id = entity.Id,
                login = entity.Login,
                jmeno = entity.Name,
                prijmeni = entity.Surname,
                datum_nastupu = entity.HireDate,
                manazer_id = supermarketEmployee.ManagerId,
                supermarket_id = supermarketEmployee.SupermarketId,
                isPokladnik = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Cashier),
                isNakladac = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.GoodsKeeper),
                isManazer = supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Manager)
            };
        }

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(zamestnanec_id), id);

        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
        
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
