using Supermarket.Core.Employees.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Login.LoggedEmployees
{
    public record LoggedSupermarketEmployee(int Id, IReadOnlyList<IEmployeeRole> Roles) : ILoggedEmployee;
}
