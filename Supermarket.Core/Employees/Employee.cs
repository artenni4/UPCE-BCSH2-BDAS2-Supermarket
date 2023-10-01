using Supermarket.Core.Common;
using Supermarket.Core.Employees.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    public class Employee : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Login { get; init; }
        public required int? SupermarketId { get; init; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordHashSalt { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required DateTimeOffset StartedWorking { get; init; }
        public required IReadOnlyList<string> Roles { get; init; }
    }
}
