using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    public class Employee
    {
        public required int Id { get; init; }
        public required string Login { get; init; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordHashSalt { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required DateTimeOffset StartedWorking { get; init; }
    }
}
