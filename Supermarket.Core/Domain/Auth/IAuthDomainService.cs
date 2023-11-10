using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.Domain.Auth
{
    /// <summary>
    /// Provides basic authentication and authorization
    /// </summary>
    public interface IAuthDomainService
    {
        /// <summary>
        /// Checks credentials and returns logged user data
        /// </summary>
        /// <returns>logged user data</returns>
        /// <exception cref="InvalidCredentialsException">in case of invalid credentials</exception>
        Task<ILoggedEmployee> AuthEmployeeAsync(LoginData loginData);
    }
}
