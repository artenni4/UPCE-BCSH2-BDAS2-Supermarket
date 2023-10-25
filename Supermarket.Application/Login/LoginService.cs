using Supermarket.Domain.Auth;
using Supermarket.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.Login
{
    internal class LoginService : ILoginService
    {
        private readonly IAuthDomainService _authDomainService;

        public LoginService(IAuthDomainService authDomainService)
        {
            _authDomainService = authDomainService;
        }

        public async Task<ILoggedEmployee> LoginEmployeeAsync(LoginData loginData)
        {
            // TODO add logs
            return await _authDomainService.AuthEmployeeAsync(loginData);
        }

        public Task<PagedResult<AdminLoginSupermarket>> GetSupermarketsAsync(RecordsRange recordsRange)
        {
            throw new NotImplementedException();
        }
    }
}
