
namespace Supermarket.Core.UseCases.Admin
{
    public class AdminAddEmployee : AdminEmployeeData
    {
        public required string Password { get; init; }
    }
}
