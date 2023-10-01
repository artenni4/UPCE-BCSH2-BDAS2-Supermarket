using Supermarket.Core.Employees.Roles;
using Supermarket.Core.Employees;
using Supermarket.Core.Login.LoggedEmployees;

namespace Supermarket.Core.Login
{
    public class LoginService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public LoginService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ILoggedEmployee> LoginEmployeeAsync(LoginData loginData)
        {
            var employee = await AuthenticateEmployee(loginData);
            return AuthorizeEmployee(employee);
        }

        /// <summary>
        /// Authenticates employee by credentials
        /// </summary>
        private async Task<Employee> AuthenticateEmployee(LoginData loginData)
        {
            var employee = await _employeeRepository.GetByLoginAsync(loginData.Login) ?? throw new InvalidCredentialsException();

            var loginPassordHash = PasswordHashing.GenerateSaltedHash(loginData.Password, employee.PasswordHashSalt);
            if (PasswordHashing.HashesAreEqual(loginPassordHash, employee.PasswordHash) == false)
            {
                throw new InvalidCredentialsException();
            }

            return employee;
        }

        /// <summary>
        /// Retrieves roles for employee
        /// </summary>
        private static ILoggedEmployee AuthorizeEmployee(Employee employee)
        {
            if (employee.Roles.Contains(DbRoleNames.SuperAdmin))
            {
                if (employee.Roles.Count > 1)
                {
                    throw new InconsistencyException($"Employee [{employee.Id}] with super admin role cannot have other roles");
                }

                return new LoggedSuperAdmin(employee.Id);
            }

            if (employee.SupermarketId.HasValue == false)
            {
                throw new InconsistencyException($"Employee [{employee.Id}] is not super admin but does not have an assigned supermarket");
            }

            var roles = employee.Roles.Select<string, IEmployeeRole>(r => r switch
            {
                DbRoleNames.Manager => new ManagerRole(employee.SupermarketId.Value),
                DbRoleNames.GoodsKeeper => new GoodsKeeperRole(employee.SupermarketId.Value),
                DbRoleNames.Cashier => new CashierRole(employee.SupermarketId.Value),
                _ => throw new InconsistencyException($"Role [{r}] does not exist")
            }).ToList();

            return new LoggedSupermarketEmployee(employee.Id, roles);
        }
    }
}
