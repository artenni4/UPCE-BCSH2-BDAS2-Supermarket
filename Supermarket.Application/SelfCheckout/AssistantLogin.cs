using Supermarket.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.SelfCheckout
{
    public class AssistantLogin
    {
        public required ILoggedEmployee Employee { get; init; }
    }
}
