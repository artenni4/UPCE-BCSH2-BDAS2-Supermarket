using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.UseCases.CashBox
{
    public class AssistantLogin
    {
        public required ILoggedEmployee Employee { get; init; }
    }
}
