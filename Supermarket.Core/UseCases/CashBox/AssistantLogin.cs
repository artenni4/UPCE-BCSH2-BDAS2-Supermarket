using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.UseCases.CashBoxes
{
    public class AssistantLogin
    {
        public required ILoggedEmployee Employee { get; init; }
    }
}
