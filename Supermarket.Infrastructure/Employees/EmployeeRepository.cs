﻿using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.GoodsKeeping;
using Supermarket.Infrastructure.StoredProducts;

namespace Supermarket.Infrastructure.Employees
{
    internal class EmployeeRepository : CrudRepositoryBase<Employee, int, DbEmployee>, IEmployeeRepository
    {
        public EmployeeRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public Task<EmployeeRole?> GetByLoginAsync(string login)
        {
            if (login != "a") return Task.FromResult((EmployeeRole?)null);

            var passwordBytes = Encoding.UTF8.GetBytes("a");
            var salt = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            byte[] hashBytes = SHA256.HashData(combinedBytes);

            return Task.FromResult((EmployeeRole?)new EmployeeRole
            {
                Id = 1,
                Name = "David",
                Surname = "Biba",
                PasswordHash = hashBytes,
                PasswordHashSalt = salt,
                Roles = new IEmployeeRole[]
                {
                    new CashierRole(SupermarketId: 1),
                    new GoodsKeeperRole(SupermarketId: 1),
                    new ManagerRole(SupermarketId: 1),
                    new AdminRole()
                },
                Login = "A",
                HireDate = new DateTimeOffset(DateTime.Today.AddDays(-7))
            });
        }

        public async Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployees(int supermarketId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId);

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    LISTAGG(r.nazev, ', ') WITHIN GROUP (ORDER BY r.nazev) AS role
                                FROM
                                    ZAMESTNANCI z
                                LEFT JOIN
                                    ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                LEFT JOIN
                                    ROLE r ON rz.role_id = r.role_id
                                WHERE
                                    z.supermarket_id = :supermarket_id
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu";

            var orderByColumns = DbEmployee.IdentityColumns
            .Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbManagerMenuEmployees>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        public async Task<ManagerMenuEmployeeDetail?> GetEmployeeDetail(int employeeId)
        {
            var parameters = new DynamicParameters().AddParameter("zamestnanec_id", employeeId);

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.login,
                                    MAX(CASE WHEN r.role_id = 1 THEN 1 ELSE 0 END) AS isPokladnik,
                                    MAX(CASE WHEN r.role_id = 2 THEN 1 ELSE 0 END) AS isNakladac,
                                    MAX(CASE WHEN r.role_id = 3 THEN 1 ELSE 0 END) AS isManazer,
                                    MAX(CASE WHEN r.role_id = 4 THEN 1 ELSE 0 END) AS isAdmin
                                FROM
                                    ZAMESTNANCI z
                                LEFT JOIN
                                    ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                LEFT JOIN
                                    ROLE r ON rz.role_id = r.role_id
                                WHERE
                                    z.zamestnanec_id = :zamestnanec_id
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login";

            var orderByColumns = DbManagerMenuEmployeeDetail.IdentityColumns
                .Select(ic => $"z.{ic}");

            DbManagerMenuEmployeeDetail? result = await _oracleConnection.QuerySingleOrDefaultAsync<DbManagerMenuEmployeeDetail>(sql, parameters);

            return result?.ToDomainEntity();
        }

        //public async Task EditEmployeeAsync(ManagerMenuEmployeeDetail employee)
        //{
        //    var parameters = new DynamicParameters()
        //        .Add
        //}

        private static IEmployeeRole[] FromDbRoles(int supermarketId, IEnumerable<string> roles)
        {
            return roles.Select<string, IEmployeeRole>(role => role switch
            {
                DbRoleNames.Manager => new ManagerRole(supermarketId),
                DbRoleNames.GoodsKeeper => new GoodsKeeperRole(supermarketId),
                DbRoleNames.Cashier => new CashierRole(supermarketId),
                DbRoleNames.SuperAdmin => new AdminRole(),
                _ => throw new RepositoryInconsistencyException($"Role [{role}] does not exist")
            }).ToArray();
        }

    }
}
