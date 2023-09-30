using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<LoggedEmployee> LoginEmployeeAsync(LoginData loginData)
        {
            var employee = await _employeeRepository.GetByLoginAsync(loginData.Login) ?? throw new InvalidCredentialsException();

            var loginPassordHash = PasswordHashing.GenerateSaltedHash(loginData.Password, employee.PasswordHashSalt);
            if (PasswordHashing.HashesAreEqual(loginPassordHash, employee.PasswordHash) == false)
            {
                throw new InvalidCredentialsException();
            }

            return LoggedEmployee.FromEmployee(employee);
        }
    }
}
