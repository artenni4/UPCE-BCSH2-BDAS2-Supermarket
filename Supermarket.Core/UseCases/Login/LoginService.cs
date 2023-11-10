using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.UseCases.Login
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
