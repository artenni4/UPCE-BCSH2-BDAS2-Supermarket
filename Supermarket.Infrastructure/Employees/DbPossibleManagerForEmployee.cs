using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Infrastructure.Employees;

public class DbPossibleManagerForEmployee
{
    public required int zamestnanec_id { get; init; }
    public required string jmeno { get; init; }
    public required string prijmeni { get; init; }

    public PossibleManagerForEmployee ToDomainEntity() => new PossibleManagerForEmployee
    {
        EmployeeId = zamestnanec_id,
        Name = jmeno,
        Surname = prijmeni
    };
}