using Supermarket.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.CashBoxes
{
    public class AssistantLogin
    {
        public required ILoggedEmployee Employee { get; init; }
    }
}
