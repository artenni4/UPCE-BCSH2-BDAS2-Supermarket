using Supermarket.Core.UseCases.ManagerMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Employees
{
    public class DbManagerMenuEmployees
    {
        public required int zamestnanec_id { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required DateTimeOffset datum_nastupu { get; init; }
        public required string role { get; init; }

        public ManagerMenuEmployee ToDomainEntity() => new ManagerMenuEmployee
        {
            EmployeeId = zamestnanec_id,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            Roles = role
        };
    }
}
