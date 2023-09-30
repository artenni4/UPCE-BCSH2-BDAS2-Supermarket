using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    public class EmployeeCredentials
    {
        public required int Id { get; init; }
        public required string Login { get; init; }
        public required byte[] Password { get; init; }
        public required byte[] Salt { get; init; }
    }
}
