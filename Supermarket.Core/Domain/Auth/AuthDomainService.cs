using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.Domain.Auth
{
    internal class AuthDomainService : IAuthDomainService
    {
        private readonly IEmployeeRepository _employeeRepository;
        
        private static readonly byte[] ReservedAdminHashSalt = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static readonly EmployeeRole ReservedAdmin = new EmployeeRole
        {
            Id = 999,
            Login = "ADMIN",
            Name = "ADMIN",
            Surname = "BEJBA",
            HireDate = DateTime.MinValue,
            PersonalNumber = null,
            PasswordHash = PasswordHashing.GenerateSaltedHash("ADMIN", ReservedAdminHashSalt),
            PasswordHashSalt = ReservedAdminHashSalt,
            RoleInfo = new Admin()
        };

        public AuthDomainService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ILoggedEmployee> AuthEmployeeAsync(LoginData loginData)
        {
            var employee = await AuthenticateEmployee(loginData);
            return AuthorizeEmployee(employee);
        }

        /// <summary>
        /// Authenticates employee by credentials
        /// </summary>
        private async Task<EmployeeRole> AuthenticateEmployee(LoginData loginData)
        {
            var employee = loginData.Login == ReservedAdmin.Login 
                ? ReservedAdmin
                : await _employeeRepository.GetRoleByLoginAsync(loginData.Login) ?? throw new InvalidCredentialsException();

            var loginPasswordHash = PasswordHashing.GenerateSaltedHash(loginData.Password, employee.PasswordHashSalt);
            if (PasswordHashing.HashesAreEqual(loginPasswordHash, employee.PasswordHash) == false)
            {
                throw new InvalidCredentialsException();
            }

            return employee;
        }

        /// <summary>
        /// Retrieves roles for employee
        /// </summary>
        private static ILoggedEmployee AuthorizeEmployee(EmployeeRole employee)
        {
            if (employee.RoleInfo is Admin)
            {
                return new LoggedAdmin(employee.Id, employee.Name, employee.Surname);
            }

            if (employee.RoleInfo is SupermarketEmployee supermarketEmployee)
            {
                return new LoggedSupermarketEmployee(employee.Id, employee.Name, employee.Surname, supermarketEmployee.SupermarketId, supermarketEmployee.Roles);
            }

            throw new NotSupportedException($"{employee.RoleInfo} is not supported role");
        }
    }
}
