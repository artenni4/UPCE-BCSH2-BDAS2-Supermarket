namespace Supermarket.Core.Domain.Auth.LoggedEmployees;

public record LoggedSupermarketEmployee(int Id, string Name, string Surname, int SupermarketId, IReadOnlyList<SupermarketEmployeeRole> Roles) : ILoggedEmployee;