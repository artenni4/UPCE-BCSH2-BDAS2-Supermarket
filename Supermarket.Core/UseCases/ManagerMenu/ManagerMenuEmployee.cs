using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuEmployee
    {
        public required int EmployeeId { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required DateTimeOffset HireDate { get; init; }
        public required string Roles { get; init; }
    }
}
