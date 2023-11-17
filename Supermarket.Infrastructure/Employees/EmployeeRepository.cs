using System.Data;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Infrastructure.Employees
{
    internal class EmployeeRepository : CrudRepositoryBase<Employee, int, DbEmployee>, IEmployeeRepository
    {
        private static readonly byte[] ReservedAdminHashSalt = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static readonly EmployeeRole ReservedAdmin = new EmployeeRole
        {
            Id = 999,
            Login = "ADMIN",
            Name = "ADMIN",
            Surname = "BEJBA",
            HireDate = DateTime.MinValue,
            PasswordHash = PasswordHashing.GenerateSaltedHash("ADMIN", ReservedAdminHashSalt),
            PasswordHashSalt = ReservedAdminHashSalt,
            RoleInfo = new Admin()
        };

        public EmployeeRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public async Task<EmployeeRole?> GetRoleByLoginAsync(string login)
        {
            if (login == ReservedAdmin.Login)
            {
                return (EmployeeRole?)ReservedAdmin;
            }
            var parameters = new DynamicParameters()
            .AddParameter("login", login);

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.login,
                                    z.manazer_id,
                                    z.supermarket_id,
                                    z.heslo_hash,
                                    z.heslo_salt,
                                    MAX(CASE WHEN r.role_id = 1 THEN 1 ELSE 0 END) AS isPokladnik,
                                    MAX(CASE WHEN r.role_id = 2 THEN 1 ELSE 0 END) AS isManazer,
                                    MAX(CASE WHEN r.role_id = 3 THEN 1 ELSE 0 END) AS isNakladac,
                                    MAX(CASE WHEN r.role_id = 4 THEN 1 ELSE 0 END) AS isAdmin
                                FROM
                                    ZAMESTNANCI z
                                LEFT JOIN
                                    ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                LEFT JOIN
                                    ROLE r ON rz.role_id = r.role_id
                                WHERE
                                    z.login = :login
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login, z.manazer_id, z.supermarket_id, z.heslo_hash, z.heslo_salt";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbLoggedEmployee>(sql, parameters);
            var foundUser = result?.ToDomainEntity();

            if (foundUser != null)
            {
                var roles = new HashSet<SupermarketEmployeeRole>();
                if (foundUser.IsCashier)
                {
                    roles.Add(SupermarketEmployeeRole.Cashier);
                }
                if (foundUser.IsGoodsKeeper)
                {
                    roles.Add(SupermarketEmployeeRole.GoodsKeeper);
                }
                if (foundUser.IsManager)
                {
                    roles.Add(SupermarketEmployeeRole.Manager);
                }

                if (!foundUser.IsAdmin)
                {
                    var emp = new EmployeeRole
                    {
                        Id = foundUser.Id,
                        Login = foundUser.Login,
                        Name = foundUser.Name,
                        Surname = foundUser.Surname,
                        HireDate = foundUser.HireDate,
                        RoleInfo = new SupermarketEmployee(foundUser.SupermarketId, foundUser.ManagerId, roles),
                        PasswordHash = foundUser.PasswordHash,
                        PasswordHashSalt = foundUser.PasswordSalt
                    };
                    return emp;
                }
                else
                {
                    var admin = new EmployeeRole
                    {
                        Id = foundUser.Id,
                        Login = foundUser.Login,
                        Name = foundUser.Name,
                        Surname = foundUser.Surname,
                        HireDate = foundUser.HireDate,
                        RoleInfo = new Admin(),
                        PasswordHash = foundUser.PasswordHash,
                        PasswordHashSalt = foundUser.PasswordSalt
                    };
                    return admin;
                }
            }
            return null;
        }

        public async Task<EmployeeRole?> GetRoleByIdAsync(int employeeId)
        {
            var parameters = new DynamicParameters()
                .AddParameter("zamestnanec_id", employeeId);

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.login,
                                    z.manazer_id,
                                    z.supermarket_id,
                                    z.heslo_hash,
                                    z.heslo_salt,
                                    MAX(CASE WHEN r.role_id = 1 THEN 1 ELSE 0 END) AS isPokladnik,
                                    MAX(CASE WHEN r.role_id = 2 THEN 1 ELSE 0 END) AS isManazer,
                                    MAX(CASE WHEN r.role_id = 3 THEN 1 ELSE 0 END) AS isNakladac,
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
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login, z.manazer_id, z.supermarket_id, z.heslo_hash, z.heslo_salt";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbLoggedEmployee>(sql, parameters);
            var foundUser = result?.ToDomainEntity();

            if (foundUser != null)
            {
                var roles = new HashSet<SupermarketEmployeeRole>();
                if (foundUser.IsCashier)
                {
                    roles.Add(SupermarketEmployeeRole.Cashier);
                }
                if (foundUser.IsGoodsKeeper)
                {
                    roles.Add(SupermarketEmployeeRole.GoodsKeeper);
                }
                if (foundUser.IsManager)
                {
                    roles.Add(SupermarketEmployeeRole.Manager);
                }

                if (!foundUser.IsAdmin)
                {
                    var emp = new EmployeeRole
                    {
                        Id = foundUser.Id,
                        Login = foundUser.Login,
                        Name = foundUser.Name,
                        Surname = foundUser.Surname,
                        HireDate = foundUser.HireDate,
                        RoleInfo = new SupermarketEmployee(foundUser.SupermarketId, foundUser.ManagerId, roles),
                        PasswordHash = foundUser.PasswordHash,
                        PasswordHashSalt = foundUser.PasswordSalt
                    };
                    return emp;
                }
                else
                {
                    var admin = new EmployeeRole
                    {
                        Id = foundUser.Id,
                        Login = foundUser.Login,
                        Name = foundUser.Name,
                        Surname = foundUser.Surname,
                        HireDate = foundUser.HireDate,
                        RoleInfo = new Admin(),
                        PasswordHash = foundUser.PasswordHash,
                        PasswordHashSalt = foundUser.PasswordSalt
                    };
                    return admin;
                }
            }
            return null;
        }

        public async Task AddAsync(EmployeeRole employeeRole)
        {
            var employeeId = await AddAndGetIdAsync(employeeRole.ToEmployee());
            await AddEmployeeRoles(employeeRole, employeeId);
        }

        public async Task UpdateAsync(EmployeeRole employeeRole)
        {
            await UpdateAsync(employeeRole.ToEmployee());
            await DeleteEmployeeRoles(employeeRole);
            await AddEmployeeRoles(employeeRole, employeeRole.Id);
        }

        public async Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployeesForManager(int employeeId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("zamestnanec_id", employeeId);

            const string sql = @"WITH podrizene AS (
                                    SELECT z.zamestnanec_id FROM zamestnanci z
                                    WHERE z.zamestnanec_id != :zamestnanec_id
                                    CONNECT BY PRIOR z.zamestnanec_id = z.manazer_id
                                    START WITH z.zamestnanec_id = :zamestnanec_id)

                                SELECT
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
                                    z.zamestnanec_id IN (SELECT * FROM podrizene)
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbManagerMenuEmployees>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        public async Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployeesForAdmin(int supermarketId, RecordsRange recordsRange)
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

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbManagerMenuEmployees>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        public async Task<ManagerMenuEmployeeDetail?> GetEmployeeDetail(int employeeId)
        {
            var parameters = new DynamicParameters().AddParameter("zamestnanec_id", employeeId);

            const string sql = @"SELECT * FROM ManagerEmployeeDetail WHERE zamestnanec_id = :zamestnanec_id";

            var orderByColumns = DbManagerMenuEmployeeDetail.IdentityColumns
                .Select(ic => $"ManagerEmployeeDetail.{ic}");

            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbManagerMenuEmployeeDetail>(sql, parameters);

            return result?.ToDomainEntity();
        }

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForManager(int employeeId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("zamestnanec_id", employeeId);

            const string sql = @"WITH manazeri AS (
                                    SELECT DISTINCT z.zamestnanec_id FROM zamestnanci z
                                    LEFT JOIN ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                    WHERE rz.role_id = 2)

                                SELECT z.zamestnanec_id, z.jmeno, z.prijmeni FROM zamestnanci z
                                WHERE z.zamestnanec_id IN (SELECT * FROM manazeri)
                                CONNECT BY PRIOR z.zamestnanec_id = z.manazer_id
                                START WITH z.zamestnanec_id = :zamestnanec_id";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbPossibleManagerForEmployee>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForAdmin(int supermarketId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("supermarket_id", supermarketId);

            const string sql = @"SELECT z.zamestnanec_id, z.jmeno, z.prijmeni FROM zamestnanci z
                                    LEFT JOIN ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                    WHERE rz.role_id = 2 AND z.supermarket_id = :supermarket_id";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbPossibleManagerForEmployee>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        private async Task DeleteEmployeeRoles(EmployeeRole employeeRole)
        {
            var deleteParameters = new DynamicParameters()
                .AddParameter("zamestnanec_id", employeeRole.Id);
            const string sqlDeleteRoles = "DELETE FROM ROLE_ZAMESTNANCU WHERE zamestnanec_id = :zamestnanec_id";
            await _oracleConnection.ExecuteAsync(sqlDeleteRoles, deleteParameters);
        }

        private async Task AddEmployeeRoles(EmployeeRole employeeRole, int employeeId)
        {
            const string sql = "INSERT INTO ROLE_ZAMESTNANCU VALUES (:role_id, :zamestnanec_id)";
            foreach (var dbEmployeeRole in DbEmployeeRole.ToDbEmployeeRoles(employeeId, employeeRole.RoleInfo))
            {
                var parameters = new DynamicParameters()
                    .AddParameter("role_id", dbEmployeeRole.role_id)
                    .AddParameter("zamestnanec_id", dbEmployeeRole.zamestnanec_id);
                await _oracleConnection.ExecuteAsync(sql, parameters);
            }
        }

        private async Task<int> AddAndGetIdAsync(Employee sale)
        {
            var dbEntity = DbEmployee.ToDbEntity(sale);
            var insertingValues = dbEntity.GetInsertingValues();

            var selector = string.Join(", ", insertingValues.ParameterNames);
            var parameters = string.Join(", ", insertingValues.ParameterNames.Select(v => ":" + v));

            var sql = $"INSERT INTO {DbEmployee.TableName} ({selector}) VALUES ({parameters}) RETURNING zamestnanec_id INTO :zamestnanec_id";

            insertingValues.Add(nameof(DbEmployee.zamestnanec_id), dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _oracleConnection.ExecuteAsync(sql, insertingValues);

            return insertingValues.Get<int>(nameof(DbEmployee.zamestnanec_id));
        }

        public async Task<PagedResult<AdminEmployee>> GetAdminMenuEmployees(RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters();

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.rodne_cislo,
                                    s.supermarket_id,
                                    s.adresa as supermarket_nazev,
                                    LISTAGG(r.nazev, ', ') WITHIN GROUP (ORDER BY r.nazev) AS role
                                FROM
                                    ZAMESTNANCI z
                                LEFT JOIN
                                    ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
                                LEFT JOIN
                                    ROLE r ON rz.role_id = r.role_id
                                LEFT JOIN
                                    SUPERMARKETY s on s.supermarket_id = z.supermarket_id
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, s.supermarket_id, s.adresa, z.rodne_cislo";

            var orderByColumns = DbEmployee.IdentityColumns.Select(ic => $"z.{ic}");

            var result = await GetPagedResult<DbAdminMenuEmployee>(recordsRange, sql, orderByColumns, parameters);

            return result.Select(dbProduct => dbProduct.ToDomainEntity());
        }

        public async Task<AdminEmployeeDetail?> GetAdminEmployeeDetail(int employeeId)
        {
            var parameters = new DynamicParameters().AddParameter("zamestnanec_id", employeeId);

            const string sql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.login,
                                    z.manazer_id,
                                    z.supermarket_id,
                                    z.rodne_cislo,
                                    MAX(CASE WHEN r.role_id = 1 THEN 1 ELSE 0 END) AS isPokladnik,
                                    MAX(CASE WHEN r.role_id = 2 THEN 1 ELSE 0 END) AS isManazer,
                                    MAX(CASE WHEN r.role_id = 3 THEN 1 ELSE 0 END) AS isNakladac,
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
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login, z.manazer_id, z.supermarket_id, z.rodne_cislo";

            var orderByColumns = DbEmployee.IdentityColumns
                .Select(ic => $"z.{ic}");

            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbAdminMenuEmployeeDetail>(sql, parameters);

            return result?.ToDomainEntity();
        }
    }
}
