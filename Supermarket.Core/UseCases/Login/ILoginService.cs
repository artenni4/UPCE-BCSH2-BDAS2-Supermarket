﻿using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.Domain.Common.Paging;

namespace Supermarket.Core.UseCases.Login
{
    /// <summary>
    /// Service for login screen
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Checks credentials and returns logged user data
        /// </summary>
        /// <returns>logged user data</returns>
        /// <exception cref="InvalidCredentialsException">in case of invalid credentials</exception>
        Task<ILoggedEmployee> LoginEmployeeAsync(LoginData loginData);
        
        /// <summary>
        /// Gets list of all supermarkets for admin to choose from
        /// </summary>
        Task<PagedResult<CustomerSupermarket>> GetSupermarketsAsync(RecordsRange recordsRange);
    }
}
