﻿using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.Domain.Auth
{
    internal class AuthDomainService : IAuthDomainService
    {
        private readonly IEmployeeRepository _employeeRepository;

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
        private async Task<Employee> AuthenticateEmployee(LoginData loginData)
        {
            var employee = await _employeeRepository.GetByLoginAsync(loginData.Login) ?? throw new InvalidCredentialsException();

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
        private static ILoggedEmployee AuthorizeEmployee(Employee employee)
        {
            if (employee.Roles.Any(role => role is AdminRole))
            {
                if (employee.Roles.Count > 1)
                {
                    throw new InconsistencyException($"Employee [{employee.Id}] with super admin role cannot have other roles");
                }

                return new LoggedAdmin(employee.Id, employee.Name, employee.Surname);
            }

            return new LoggedSupermarketEmployee(employee.Id, employee.Name, employee.Surname, employee.Roles);
        }
    }
}