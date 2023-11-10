namespace Supermarket.Core.Domain.Auth
{
    public class LoginData
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
    }
}
