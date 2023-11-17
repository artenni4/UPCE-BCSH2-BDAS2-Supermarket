namespace Supermarket.Core.UseCases.ManagerMenu;

public class PossibleManagerForEmployee
{
    public required int EmployeeId { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string FullName => $"{Name} {Surname}";
}