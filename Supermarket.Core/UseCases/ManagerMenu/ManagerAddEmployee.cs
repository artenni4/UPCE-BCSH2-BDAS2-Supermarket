namespace Supermarket.Core.UseCases.ManagerMenu;

public class ManagerAddEmployee : ManagerEmployeeData
{
    public required string Password { get; init; }
}