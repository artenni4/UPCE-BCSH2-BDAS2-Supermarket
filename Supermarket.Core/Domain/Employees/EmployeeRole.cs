using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.Domain.Employees
{
    public class EmployeeRole : IEntity<int>
    {
        public required int Id { get; init; }
        public required IEmployeeRoleInfo RoleInfo { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordHashSalt { get; init; }
        public required string Login { get; init; }
        public required DateTime HireDate { get; init; }
        public required string? PersonalNumber { get; init; }
        
        public Employee ToEmployee()
        {
            var supermarketEmployee = RoleInfo as SupermarketEmployee;
            
            return new Employee
            {
                Id = Id,
                Login = Login,
                Name = Name,
                Surname = Surname,
                ManagerId = supermarketEmployee?.ManagerId,
                PasswordHash = PasswordHash,
                PasswordHashSalt = PasswordHashSalt,
                StartedWorking = HireDate,
                SupermarketId = supermarketEmployee?.SupermarketId,
                PersonalNumber = PersonalNumber
            };
        }
    }
}
