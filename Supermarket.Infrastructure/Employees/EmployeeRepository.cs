using System.Data;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Types;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.Login;

namespace Supermarket.Infrastructure.Employees
{
    internal class EmployeeRepository : CrudRepositoryBase<Employee, int, DbEmployee>, IEmployeeRepository
    {
        public EmployeeRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }
        
        const string employeeRoleSql = @"SELECT
                                    z.zamestnanec_id,
                                    z.jmeno,
                                    z.prijmeni,
                                    z.datum_nastupu,
                                    z.login,
                                    z.manazer_id,
                                    z.supermarket_id,
                                    z.heslo_hash,
                                    z.heslo_salt,
                                    je_role_zamestnancu(z.zamestnanec_id, 'Pokladnik') AS isPokladnik,
                                    je_role_zamestnancu(z.zamestnanec_id, 'Manazer') AS isManazer,
                                    je_role_zamestnancu(z.zamestnanec_id, 'Nakladac') AS isNakladac,
                                    je_role_zamestnancu(z.zamestnanec_id, 'Admin') AS isAdmin
                                FROM
                                    ZAMESTNANCI z
                                WHERE {0}
                                GROUP BY
                                    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login, z.manazer_id, z.supermarket_id, z.heslo_hash, z.heslo_salt";

        public async Task<EmployeeRole?> GetRoleByLoginAsync(string login)
        {
            var parameters = new DynamicParameters().AddParameter("login", login);
            var sql = string.Format(employeeRoleSql, "z.login = :login");
            
            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbLoggedEmployee>(sql, parameters);
            return result?.ToDomainEntity();
        }

        public async Task<EmployeeRole?> GetRoleByIdAsync(int employeeId)
        {
            var parameters = new DynamicParameters().AddParameter("zamestnanec_id", employeeId);
            var sql = string.Format(employeeRoleSql, "z.zamestnanec_id = :zamestnanec_id");

            var result = await _oracleConnection.QuerySingleOrDefaultAsync<DbLoggedEmployee>(sql, parameters);
            return result?.ToDomainEntity();
        }

        public async Task AddAsync(EmployeeRole employeeRole)
        {
            var employeeId = await AddAndGetIdAsync(employeeRole.ToEmployee());
            await AddEmployeeRoles(employeeRole, employeeId);
        }

        public async Task UpdateAsync(EmployeeRole employeeRole)
        {
            var parameters = new DynamicParameters()
                .AddParameter("p_zamestnanec_id", employeeRole.Id)
                .AddParameter("p_jmeno", employeeRole.Name)
                .AddParameter("p_prijmeni", employeeRole.Surname)
                .AddParameter("p_login", employeeRole.Login)
                .AddParameter("p_datum_nastupu", employeeRole.HireDate)
                .AddParameter("p_heslo_hash", employeeRole.PasswordHash)
                .AddParameter("p_heslo_salt", employeeRole.PasswordHashSalt)
                .AddParameter("p_rodne_cislo", employeeRole.PersonalNumber);
            
            if (employeeRole.RoleInfo is Admin)
            {
                parameters.AddParameter("p_je_admin", 1);
                parameters.AddParameter("p_je_pokladnik", 0);
                parameters.AddParameter("p_je_nakladac", 0);
                parameters.AddParameter("p_je_manazer", 0);
            }
            else if (employeeRole.RoleInfo is SupermarketEmployee supermarketEmployee)
            {
                parameters.AddParameter("p_je_admin", 0);
                parameters.AddParameter("p_je_pokladnik", 
                    supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Cashier) ? 1 : 0);
                
                parameters.AddParameter("p_je_nakladac",
                    supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.GoodsKeeper) ? 1 : 0);
                
                parameters.AddParameter("p_je_manazer", 
                    supermarketEmployee.Roles.Contains(SupermarketEmployeeRole.Manager) ? 1 : 0);
            }
            else
            {
                throw new NotSupportedException($"{employeeRole.RoleInfo} is not supported.");
            }
            
            await _oracleConnection.ExecuteAsync("EDIT_ZAMESTNANCE", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployeesForManager(int managerId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("zamestnanec_id", managerId);

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

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForManager(int managerId, RecordsRange recordsRange)
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("v_manazeri_rc", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.ReturnValue); 
            parameters.Add("p_manazer_id", managerId);

            var result = await _oracleConnection
                .QueryAsync<DbPossibleManagerForEmployee>("DEJ_MOZNE_MANAZERY", parameters, commandType: CommandType.StoredProcedure);

            var items = result
                .Select(dbProduct => dbProduct.ToDomainEntity())
                .ToArray();

            return new PagedResult<PossibleManagerForEmployee>(items, recordsRange, items.Length);
        }

        public async Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForAdmin(int supermarketId, RecordsRange recordsRange)
        {
            var parameters = new DynamicParameters()
                .AddParameter("supermarket_id", supermarketId);

            const string sql = @"SELECT z.zamestnanec_id, z.jmeno, z.prijmeni FROM zamestnanci z 
                                 WHERE
                                     je_role_zamestnancu(z.zamestnanec_id, 'Manazer') = 1 AND
                                     z.supermarket_id = :supermarket_id";

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
    }
}
