using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.Supermarkets;

namespace Supermarket.Core.UseCases.Login
{
    internal class LoginService : ILoginService
    {
        private readonly IAuthDomainService _authDomainService;
        private readonly ISupermarketRepository _supermarketRepository;

        public LoginService(IAuthDomainService authDomainService, ISupermarketRepository supermarketRepository)
        {
            _authDomainService = authDomainService;
            _supermarketRepository = supermarketRepository;
        }

        public async Task<ILoggedEmployee> LoginEmployeeAsync(LoginData loginData)
        {
            // TODO add logs
            return await _authDomainService.AuthEmployeeAsync(loginData);
        }

        public async Task<PagedResult<AdminLoginSupermarket>> GetSupermarketsAsync(RecordsRange recordsRange)
        {
            var supermarkets = await _supermarketRepository.GetPagedAsync(recordsRange);
            return supermarkets.Select(s => new AdminLoginSupermarket
            {
                Id = s.Id,
                Address = s.Address
            });
        }
    }
}
