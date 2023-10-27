using Dapper;
using Supermarket.Domain.Employees;

namespace Supermarket.Infrastructure.Employees;

internal class DbEmployee : IDbEntity<Employee, int, DbEmployee>
{
    public required int zamestnanec_id { get; init; }
    
    public static string TableName => "ZAMESTNANCI";
    public static IReadOnlyList<string> IdentityColumns { get; } = new[]
    {
        nameof(zamestnanec_id)
    };

    public Employee ToDomainEntity()
    {
        throw new NotImplementedException();
    }

    public static DbEmployee MapToDbEntity(Employee entity)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetEntityIdParameters(int id)
    {
        throw new NotImplementedException();
    }

    public static DynamicParameters GetOutputIdentityParameters()
    {
        throw new NotImplementedException();
    }

    public static int ExtractIdentity(DynamicParameters dynamicParameters) =>
        dynamicParameters.Get<int>(nameof(zamestnanec_id));
}