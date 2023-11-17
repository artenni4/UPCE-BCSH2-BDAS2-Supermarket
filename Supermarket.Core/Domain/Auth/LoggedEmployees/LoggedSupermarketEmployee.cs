namespace Supermarket.Core.Domain.Auth.LoggedEmployees;

public record LoggedSupermarketEmployee(int Id, string Name, string Surname, int SupermarketId, IReadOnlySet<SupermarketEmployeeRole> Roles) : ILoggedEmployee;