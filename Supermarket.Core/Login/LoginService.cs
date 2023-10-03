using Supermarket.Core.Auth;
using Supermarket.Core.Auth.LoggedEmployees;

namespace Supermarket.Core.Login
{
    public class LoginService : ILoginService
    {
        private readonly IAuthService _authService;

        public LoginService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ILoggedEmployee> LoginEmployeeAsync(LoginData loginData)
        {
            // TODO add logs
            return await _authService.AuthEmployeeAsync(loginData);
        }
    }
}
