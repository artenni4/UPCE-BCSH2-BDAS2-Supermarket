namespace Supermarket.Core.UseCases.ManagerMenu;

public class ManagerAddEmployee : ManagerEmployeeData
{
    public required int SupermarketId { get; init; }
    public required string Password { get; init; }
}