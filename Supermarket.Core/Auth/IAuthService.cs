using Supermarket.Core.Auth.LoggedEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Auth
{
    /// <summary>
    /// Provides basic authentication and authorization
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Checks credentials and returns logged user data
        /// </summary>
        /// <returns>logged user data</returns>
        /// <exception cref="InvalidCredentialsException">in case of invalid credentials</exception>
        Task<ILoggedEmployee> AuthEmployeeAsync(LoginData loginData);
    }
}
