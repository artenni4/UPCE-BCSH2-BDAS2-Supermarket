using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Auth.LoggedEmployees
{
    /// <summary>
    /// Marker for logged employee
    /// </summary>
    public interface ILoggedEmployee
    {
        int Id { get; }
    }
}
