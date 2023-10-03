﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Core.Auth;
using Supermarket.Core.Auth.LoggedEmployees;

namespace Supermarket.Core.Login
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
    }
}
