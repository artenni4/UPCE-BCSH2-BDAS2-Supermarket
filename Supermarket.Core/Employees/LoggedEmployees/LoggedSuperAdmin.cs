using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees.LoggedEmployees
{
    public record LoggedSuperAdmin(int Id) : ILoggedEmployee;
}
