using Supermarket.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    public class LoggedEmployee : IEntity<int>
    {
        public required int Id { get; init; }

        public static LoggedEmployee FromEmployee(Employee employee) => new ()
        {
            Id = employee.Id
        };
    }
}
